﻿using FluentScheduler;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
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
        public static IFactory<ICommand> commandFactory;
        public static IScreenCapture screenCaptureSchedule;
        public static ITempMon tempMonWarning;
        public static ITempMon tempMonSchedule;
        public static ITempMon[] tempMons;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Task RefreshTask = Task.Factory.StartNew(() =>
            {
                while (!_shouldStop)
                {
                    pSDK.RefreshInformation();
                    Thread.Sleep(1000);
                }
            });

            screenCaptureSchedule = new ScreenCaptureSchedule();

            tempMonWarning = new TempMonWarning
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            tempMonSchedule = new TempMonSchedule
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            tempMons = new ITempMon[]
            {
                tempMonWarning,
                tempMonSchedule
            };

            MainForm mainForm = new MainForm();

            string token = TelegramSettings.GetBotToken();
            int id = TelegramSettings.GetAdminId();

            TelebotClient telebotClient = new TelebotClient(token, id);

            var presenter = new MainFormPresenter(
                mainForm,
                telebotClient,
                screenCaptureSchedule,
                tempMonWarning,
                tempMonSchedule
            );

            SettingsBase.AddProfiles(
                presenter,
                (Settings.IProfile)tempMonWarning
            );

            buildCommandFactory();

            Application.Run(mainForm);

            // commit profiles changes
            SettingsBase.CommitChanges();

            // write changes to disk
            SettingsBase.WriteChanges();

            // stop job manager and remove all jobs
            JobManager.Stop();
            JobManager.RemoveAllJobs();

            // wait for task to complete
            _shouldStop = true;
            RefreshTask.Wait();
        }

        private static void buildCommandFactory()
        {
            commandFactory = new CommandFactory
            (
                new ICommand[]
                {
                    new StatusCmd
                    (
                        new IStatus[]
                        {
                            new SystemStatus
                            (
                                DeviceFactory.RAMDevices,
                                DeviceFactory.CPUDevices,
                                DeviceFactory.HDDDevices,
                                DeviceFactory.GPUDevices
                            ),
                            new IPAddrStatus(),
                            new UptimeStatus(),
                            new MonitorsStatus(),
                            new CaptureStatus()
                        }
                    ),
                    new AppsCmd(),
                    new BrightCmd(),
                    new CaptureCmd(),
                    new CapAppCmd(),
                    new CapTimeCmd(),
                    new ScreenCmd(),
                    new TempMonCmd(),
                    new TempTimeCmd(),
                    new PowerCmd(),
                    new ShutdownCmd(),
                    new MessageBoxCmd(),
                    new KillTaskCmd(),
                    new VolCmd(),
                    new SpecCmd(),
                    new HelpCmd()
                }
            );
        }
    }
}
