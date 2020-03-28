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
using Telebot.Intranet;
using Telebot.Jobs;
using Telebot.Jobs.Intranet;
using Telebot.Presenters;
using Telebot.Settings;
using Telebot.Temperature;
using static CPUID.CpuIdWrapper64;

namespace Telebot
{
    static class Program
    {
        public static SettingsFactory Settings { get; private set; }
        public static IFactory<IInetBase> InetFactory { get; private set; }
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

            InetFactory = new InetFactory();
            TempFactory = new TempFactory();
            CaptureFactory = new CaptureFactory();

            CommandFactory = BuildCommandFactory();

            MainView mainView = new MainView();
            IBotClient botClient = new Clients.Telebot(token, id);

            var inetJobs = InetFactory.GetAllEntities();
            var captureJobs = CaptureFactory.GetAllEntities();
            var temperatureJobs = TempFactory.GetAllEntities();

            var presenter = new MainViewPresenter(
                mainView,
                botClient,
                inetJobs,
                captureJobs,
                temperatureJobs
            );

            JobManager.AddJob(
                () =>
                {
                    Sdk64.RefreshInformation();
                }, (s) => s.WithName("RefreshInformation").ToRunNow().AndEvery(1).Seconds()
            );

            Application.ApplicationExit += ApplicationExit;

            Application.Run(mainView);
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

            Sdk64.UninitSDK();
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
                new KillCommand(),
                new SpecCommand(),
                new VolCommand(),
                new RestartCommand(),
                new ExitCommand(),
                new UpdateCommand(),
                new HelpCommand()
            };

            var osver = Environment.OSVersion.Version;

            // we select only compatible commands with current windows version.
            var compatibleCommands = commands.Where(c => c.OSVersion < osver);

            return new CommandFactory(compatibleCommands);
        }
    }
}
