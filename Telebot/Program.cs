using Common;
using FluentScheduler;
using System;
using System.Linq;
using System.Windows.Forms;
using Telebot.Capture;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Contracts;
using Telebot.Intranet;
using Telebot.Presenters;
using Telebot.Settings;
using Telebot.Temperature;
using static CPUID.CpuIdWrapper64;

namespace Telebot
{
    static class Program
    {
        public static SettingsFactory Settings { get; private set; }
        public static IINetMonitor LanMonitor { get; private set; }
        public static IInetScanner LanScanner { get; private set; }
        public static IFactory<ICommand> CommandFactory { get; private set; }
        public static IFactory<IJob<TempArgs>> TempFactory { get; private set; }
        public static IFactory<IJob<CaptureArgs>> CaptureFactory { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings = new SettingsFactory();

            string token = Settings.Telegram.GetBotToken();
            int id = Settings.Telegram.GetAdminId();

            if (string.IsNullOrEmpty(token) || id == 0)
            {
                MessageBox.Show(
                    "Missing Token or AdminId in settings.ini.",
                    "Missing info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            TempFactory = new TempFactory();
            CaptureFactory = new CaptureFactory();

            LanScanner = new LanScanner();
            LanMonitor = new LanMonitor();

            CommandFactory = BuildCommandFactory();

            MainView view = new MainView();
            IBotClient client = new Clients.Telebot(token, id);

            var captures = CaptureFactory.GetAllEntities();
            var temperatures = TempFactory.GetAllEntities();

            var presenter = new MainViewPresenter(
                view,
                client,
                LanScanner,
                LanMonitor,
                captures,
                temperatures
            );

            JobManager.AddJob(
                () =>
                {
                    Sdk64.RefreshInformation();
                }, (s) => s.WithName("RefreshInformation").ToRunNow().AndEvery(1).Seconds()
            );

            Application.ApplicationExit += ApplicationExit;

            Application.Run(view);
        }

        private static void ApplicationExit(object sender, EventArgs e)
        {
            int jobsCount = JobManager.AllSchedules.Count();

            if (jobsCount > 0)
            {
                // todo - save ongoing jobs to disk and reload them?

                JobManager.StopAndBlock();
                JobManager.RemoveAllJobs();
            }

            Settings.Main.CommitChanges();

            Settings.Main.WriteChanges();
        }

        private static CommandFactory BuildCommandFactory()
        {
            var devices = DeviceFactory.GetAllEntities();

            var statuses = new IStatus[]
            {
                new SystemStatus(devices),
                new LanAddrStatus(),
                new WanAddrStatus(),
                new UptimeStatus(),
                new LanMonStatus(),
                new TempStatus(),
                new CapsStatus()
            };

            var commands = new ICommand[]
            {
                new StatusCommand(statuses),
                new AppsCommand(),
                new BrightCommand(),
                new CapAppCommand(),
                new CapTimeCommand(),
                new CapCommand(),
                new ScreenCommand(),
                new TempWarnCommand(),
                new TempTimeCommand(),
                new PowerCommand(),
                new ShutdownCommand(),
                new AlertCommand(),
                new LanCommand(),
                new KillTaskCommand(),
                new SpecCommand(),
                new VolCommand(),
                new RestartCommand(),
                new ExitCommand(),
                new UpdateCommand(),
                new HelpCommand()
            };

            return new CommandFactory(commands);
        }
    }
}
