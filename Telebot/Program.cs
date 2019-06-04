using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.Commands;
using Telebot.Commands.Factories;
using Telebot.Commands.StatusCommands;
using Telebot.DeviceProviders;
using Telebot.Loggers;
using Telebot.Presenters;
using Telebot.Settings;
using Telebot.StatusCommands;

namespace Telebot
{
    static class Program
    {
        public static CPUIDSDK pSDK;

        public static ILogger logger;
        public static ISettings appSettings;
        public static CommandFactory commandFactory;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var RefreshThread = new Thread(RefreshThreadProc);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            bool sdkLoaded = pSDK.InitSDK_Quick();

            if (!sdkLoaded)
            {
                throw new Exception("Couldn't load cpuidsdk module.");
            }

            RefreshThread.Start();

            logger = new FileLogger();
            appSettings = new SettingsManager();

            commandFactory = new CommandFactory
            (
                new ICommand[]
                {
                    new StatusCmd
                    (
                        new IStatusCommand[]
                        {
                            new SystemCmd
                            (    
                                ProvidersFactory.GetRAMProviders(),
                                ProvidersFactory.GetCPUProviders(),
                                ProvidersFactory.GetDriveProviders(),
                                ProvidersFactory.GetGPUProviders()
                            ),
                            new IPCmd(),
                            new UptimeCmd(),
                            new TempMonitorCmd()
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

            MainForm mainForm = new MainForm();

            var presenter = new MainFormPresenter(mainForm);

            Application.Run(mainForm);

            _shouldStop = true;
            RefreshThread.Join();

            pSDK.UninitSDK();

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
    }
}
