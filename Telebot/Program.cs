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
using Telebot.Presenters;
using Telebot.Services;
using Telebot.StatusCommands;

namespace Telebot
{
    static class Program
    {
        public static ILogger logger;
        public static ISettings appSettings;
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

                logger = new FileLogger();
                appSettings = new SettingsManager();
                var mainForm = new MainForm();

                var presenter = new MainFormPresenter(mainForm, new TelegramService());

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
                typeof(CapAppCmd),
                typeof(CapTimeCmd),
                typeof(ScreenCmd),
                typeof(TempMonCmd),
                typeof(TempTimeCmd),
                typeof(PowerCmd),
                typeof(ShutdownCmd),
                typeof(MessageBoxCmd),
                typeof(KillTaskCmd),
                typeof(VolCmd),
                typeof(SpecCmd),
                typeof(HelpCmd)
            );

            container.Collection.Register<IHardwareProvider>
            (
                typeof(CPUProvider),
                typeof(GPUProvider)
            );

            container.Register<CaptureLogic>(Lifestyle.Singleton);
            container.Register<NetworkLogic>(Lifestyle.Singleton);
            container.Register<PowerLogic>(Lifestyle.Singleton);
            container.Register<DisplayLogic>(Lifestyle.Singleton);
            container.Register<SystemLogic>(Lifestyle.Singleton);
            container.Register<WindowsLogic>(Lifestyle.Singleton);
            container.Register<MediaLogic>(Lifestyle.Singleton);
        }
    }
}
