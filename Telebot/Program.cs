using CPUID.Factories;
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

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var RefreshThread = new Thread(RefreshThreadProc);

            RefreshThread.Start();

            logger = new FileLogger();
            appSettings = new SettingsManager();

            string admin_token = appSettings.TelegramToken;
            int admin_id = appSettings.TelegramAdminId;

            MainForm mainForm = new MainForm();
            TelebotClient telebotClient = new TelebotClient(admin_token, admin_id);

            screenCapture = new ScreenCaptureDurated();

            temperatureMonitorWarning = new TemperatureMonitorWarning
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            temperatureMonitorDurated = new TemperatureMonitorDurated
            (
                DeviceFactory.CPUDevices,
                DeviceFactory.GPUDevices
            );

            var temperatureMonitors = new ITemperatureMonitor[]
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
                                DeviceFactory.RAMDevices,
                                DeviceFactory.CPUDevices,
                                DeviceFactory.HDDDevices,
                                DeviceFactory.GPUDevices
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
