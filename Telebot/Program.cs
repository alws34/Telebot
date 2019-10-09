﻿using CPUID.Builder;
using FluentScheduler;
using System;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Builder;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Commands.Status.Builder;
using Telebot.Contracts;
using Telebot.Presenters;
using Telebot.ScreenCapture;
using Telebot.Temperature;
using static CPUID.CPUIDCore;
using static Telebot.Settings.SettingsFactory;

namespace Telebot
{
    static class Program
    {
        public static IFactory<ICommand> CmdFactory { get; private set; }
        public static IFactory<IJob<TempChangedArgs>> TempFactory { get; private set; }
        public static IFactory<IJob<ScreenCaptureArgs>> ScreenFactory { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string token = TelegramSettings.GetBotToken();
            int id = TelegramSettings.GetAdminId();

            if (string.IsNullOrEmpty(token) || id == 0)
            {
                MessageBox.Show(
                    "Please fill token and admin id in settings.ini.",
                    "Missing info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            TelebotClient telebotClient = new TelebotClient(token, id);

            TempFactory = new TempMonFactory();
            ScreenFactory = new ScreenCapFactory();

            buildCommandFactory();

            var tempMons = TempFactory.GetAllEntities();
            var screenCaps = ScreenFactory.GetAllEntities();

            MainForm mainForm = new MainForm();

            var presenter = new MainFormPresenter(
                mainForm,
                telebotClient,
                screenCaps,
                tempMons
            );

            JobManager.AddJob(() =>
            {
                pSDK.RefreshInformation();
            }, (s) => s.ToRunNow().AndEvery(1).Seconds());

            Application.Run(mainForm);

            SettingsBase.CommitChanges();

            SettingsBase.WriteChanges();

            JobManager.StopAndBlock();
        }

        private static void buildCommandFactory()
        {
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.RAMDevices)
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.HDDDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            var statuses = new StatusBuilder()
                .Add(new SystemStatus(devices))
                .Add(new IPAddrStatus())
                .Add(new UptimeStatus())
                .Add(new MonitorsStatus())
                .Add(new CaptureStatus())
                .Build();

            var commands = new CmdBuilder()
                .Add(new StatusCmd(statuses))
                .Add(new AppsCmd())
                .Add(new BrightCmd())
                .Add(new CaptureCmd())
                .Add(new CapAppCmd())
                .Add(new CapTimeCmd())
                .Add(new ScreenCmd())
                .Add(new TempMonCmd())
                .Add(new TempTimeCmd())
                .Add(new PowerCmd())
                .Add(new ShutdownCmd())
                .Add(new MessageBoxCmd())
                .Add(new KillTaskCmd())
                .Add(new VolCmd())
                .Add(new SpecCmd())
                .Add(new HelpCmd())
                .Build();

            CmdFactory = new CommandFactory(commands);
        }
    }
}
