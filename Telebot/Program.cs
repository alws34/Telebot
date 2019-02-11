using SimpleInjector;
using System;
using System.Threading;
using System.Windows.Forms;
using Telebot.BusinessLogic;
using Telebot.Commands;
using Telebot.Commands.StatusCommands;
using Telebot.HwProviders;
using Telebot.Loggers;
using Telebot.Managers;
using Telebot.Monitors;
using Telebot.Presenters;
using Telebot.ScheduledOperations;
using Telebot.Services;
using Telebot.StatusCommands;

namespace Telebot
{
    static class Program
    {
        public static Container container;
        public static CPUIDSDK pSDK;
        public static int NbDevices;
        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread UpdateThread = new Thread(ThreadLoop);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            bool res = pSDK.InitSDK_Quick();

            if (res)
            {
                NbDevices = pSDK.GetNumberOfDevices();

                UpdateThread.Start();

                buildContainer();

                var mainForm = container.GetInstance<MainForm>();

                var presenter = new MainFormPresenter(mainForm);

                Application.Run(mainForm);

                _shouldStop = true;
                UpdateThread.Join();

                pSDK.UninitSDK();
            }
        }

        private static void ThreadLoop()
        {
            while (!_shouldStop)
            {
                pSDK.RefreshInformation();
                Thread.Sleep(1000);
            }
        }

        private static void buildContainer()
        {
            container = new Container();

            container.Collection.Register<IStatusCommand>
            (
                typeof(SystemCmd),
                typeof(IPCmd),
                typeof(UptimeCmd),
                typeof(TempMonitorCmd)
            );

            container.Collection.Register<ICommand>
            (
                typeof(StatusCmd),
                typeof(AppsCmd),
                typeof(CaptureCmd),
                typeof(CapTimeCmd),
                typeof(ScreenOnCmd),
                typeof(ScreenOffCmd),
                typeof(TempMonOnCmd),
                typeof(TempMonOffCmd),
                typeof(TempTimeCmd),
                typeof(RebootCmd),
                typeof(ShutdownCmd),
                typeof(SleepCmd),
                typeof(LockCmd),
                typeof(HelpCmd)
            );

            container.Collection.Register<ITemperatureProvider>
            (
                typeof(CPUProvider),
                typeof(GPUProvider)
            );

            container.Register<IScheduledScreenCapture, ScheduledScreenCapture>(Lifestyle.Singleton);
            container.Register<IScheduledTemperatureMonitor, ScheduledSystemTempMonitor>(Lifestyle.Singleton);
            container.Register<ITemperatureMonitor, SystemTempMonitor>(Lifestyle.Singleton);
            container.Register<ICommunicationService, TelegramService>(Lifestyle.Singleton);
            container.Register<ISettings, SettingsManager>(Lifestyle.Singleton);
            container.Register<ILogger, FileLogger>(Lifestyle.Singleton);
            container.Register<MainForm>(Lifestyle.Singleton);

            container.Register<CaptureLogic>(Lifestyle.Singleton);
            container.Register<NetworkLogic>(Lifestyle.Singleton);
            container.Register<PowerLogic>(Lifestyle.Singleton);
            container.Register<DisplayLogic>(Lifestyle.Singleton);
            container.Register<SystemLogic>(Lifestyle.Singleton);
            container.Register<WindowsLogic>(Lifestyle.Singleton);
        }
    }
}
