using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.Status;
using Telebot.Loggers;
using Telebot.Presenters;
using Telebot.ScreenCapture;
using Telebot.Settings;
using Telebot.Temperature;

using static CPUID.CPUIDCore;
using static CPUID.Factories.DeviceFactory;

namespace Telebot
{
    static class Program
    {
        public static ILogger logger;
        public static ISettings appSettings;

        public static CommandFactory commandFactory;
        public static IScreenCapture screenCapture;
        public static ITemperatureMonitor temperatureMonitorWarning;
        public static ITemperatureMonitor temperatureMonitorDurated;
        public static ITemperatureMonitor[] temperatureMonitors;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread RefreshThread = new Thread(RefreshThreadProc);

            RefreshThread.Start();

            logger = new FileLogger();
            appSettings = new SettingsManager();

            string bot_token = appSettings.TelegramToken;
            int admin_id = appSettings.TelegramAdminId;

            MainForm mainForm = new MainForm();
            TelebotClient telebotClient = new TelebotClient(bot_token, admin_id);

            screenCapture = new ScreenCaptureDurated();

            temperatureMonitorWarning = new TemperatureMonitorWarning
            (
                CPUDevices,
                GPUDevices
            );

            temperatureMonitorDurated = new TemperatureMonitorDurated
            (
                CPUDevices,
                GPUDevices
            );

            temperatureMonitors = new ITemperatureMonitor[]
            {
                temperatureMonitorWarning,
                temperatureMonitorDurated
            };

            var presenter = new MainFormPresenter
            (
                mainForm,
                telebotClient,
                screenCapture,
                temperatureMonitors
            );

            buildCommandFactory();

            Application.Run(mainForm);

            _shouldStop = true;
            RefreshThread.Join();

            // wait for finalizers to "save" settings before committing
            GC.WaitForPendingFinalizers();

            // commit settings changes to disk
            appSettings.CommitChanges();
        }

        private static void RefreshThreadProc()
        {
            while (!_shouldStop)
            {
                pSDK.RefreshInformation();
                Thread.Sleep(1000);
            }
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
                                RAMDevices,
                                CPUDevices,
                                HDDDevices,
                                GPUDevices
                            ),
                            new IPAddrStatus(),
                            new UptimeStatus(),
                            new MonitorsStatus()
                        }
                    ),
                    new AppsCmd(),
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
