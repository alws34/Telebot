using Common;
using CPUID.Builder;
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
using Telebot.Network;
using Telebot.Presenters;
using Telebot.ScreenCapture;
using Telebot.Settings;
using Telebot.Temperature;
using static CPUID.CPUIDCore;

namespace Telebot
{
    static class Program
    {
        public static bool FirstRun { get; private set; }
        public static SettingsFactory Settings { get; private set; }
        public static INetMonitor NetMonitor { get; private set; }
        public static IFactory<ICommand> CmdFactory { get; private set; }
        public static IFactory<IJob<TempChangedArgs>> TempFactory { get; private set; }
        public static IFactory<IJob<ScreenCaptureArgs>> ScreenFactory { get; private set; }

        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

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

            FirstRun = Properties.Settings.Default.FirstRun;

            IBotClient client = new Clients.Telebot(token, id);
            NetMonitor = new LanMonitor();

            TempFactory = new TempMonFactory();
            ScreenFactory = new ScreenCapFactory();
            CmdFactory = BuildCommandFactory();

            MainView mainView = new MainView();

            var presenter = new MainViewPresenter(
                mainView,
                client,
                NetMonitor
            );

            JobManager.AddJob(() => { Sdk.RefreshInformation(); },
                (s) => s.ToRunNow().AndEvery(1).Seconds()
            );

            Application.Run(mainView);

            if (FirstRun)
            {
                Properties.Settings.Default["FirstRun"] = false;
                Properties.Settings.Default.Save();
            }

            Settings.Handler.CommitChanges();

            Settings.Handler.WriteChanges();

            JobManager.StopAndBlock();
        }

        private static CommandFactory BuildCommandFactory()
        {
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.RAMDevices)
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.HDDDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            var statuses = new StatusBuilder()
                .Add(new SystemStatus(devices))
                .Add(new LanAddrStatus())
                .Add(new WanAddrStatus())
                .Add(new UptimeStatus())
                .Add(new TemMonStatus())
                .Add(new ScrnCapStatus())
                .Build();

            var commands = new CmdBuilder()
                .Add(new StatusCmd(statuses))
                .Add(new AppsCmd())
                .Add(new BrightCmd())
                .Add(new CapAppCmd())
                .Add(new CapTimeCmd())
                .Add(new CaptureCmd())
                .Add(new ScreenCmd())
                .Add(new TempMonCmd())
                .Add(new TempTimeCmd())
                .Add(new PowerCmd())
                .Add(new ShutdownCmd())
                .Add(new MessageBoxCmd())
                .Add(new LanMonCmd())
                .Add(new KillTaskCmd())
                .Add(new SpecCmd())
                .Add(new VolCmd())
                .Add(new RestartCmd())
                .Add(new ExitCmd())
                .Add(new HelpCmd())
                .Build();

            return new CommandFactory(commands);
        }
    }
}
