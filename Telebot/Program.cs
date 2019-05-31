using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telebot.BusinessLogic;
using Telebot.Commands;
using Telebot.Commands.StatusCommands;
using Telebot.HwProviders;
using Telebot.Loggers;
using Telebot.Settings;
using Telebot.Presenters;
using Telebot.Clients;
using Telebot.StatusCommands;

namespace Telebot
{
    static class Program
    {
        public static ILogger logger;
        public static ISettings appSettings;
        public static Container container;
        public static CPUIDSDK pSDK;

        private static volatile bool _shouldStop = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var UpdateThread = new Thread(ThreadLoop);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            bool sdkLoaded = pSDK.InitSDK_Quick();

            if (!sdkLoaded)
            {
                throw new Exception("Couldn't load cpuidsdk module.");
            }

            UpdateThread.Start();

            buildContainer();

            logger = new FileLogger();
            appSettings = new SettingsManager();
            MainForm mainForm = new MainForm();

            var presenter = new MainFormPresenter(mainForm);

            Application.Run(mainForm);

            _shouldStop = true;
            UpdateThread.Join();

            pSDK.UninitSDK();

            appSettings.CommitChanges();
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

            var providers = GetCPUProviders().Concat(GetGPUProviders());
            container.Collection.Register<IDeviceProvider>(providers);

            container.Register<CaptureLogic>(Lifestyle.Singleton);
            container.Register<NetworkLogic>(Lifestyle.Singleton);
            container.Register<PowerLogic>(Lifestyle.Singleton);
            container.Register<DisplayLogic>(Lifestyle.Singleton);
            container.Register<SystemLogic>(Lifestyle.Singleton);
            container.Register<WindowsLogic>(Lifestyle.Singleton);
            container.Register<MediaLogic>(Lifestyle.Singleton);
        }

        private static IDeviceProvider[] GetCPUProviders()
        {
            int cpu_count = pSDK.GetNumberOfProcessors();

            var arr = new List<IDeviceProvider>(cpu_count);

            for (int idxDevice = 0; idxDevice < pSDK.GetNumberOfDevices(); idxDevice++)
            {
                if (pSDK.GetDeviceClass(idxDevice) == CPUIDSDK.CLASS_DEVICE_PROCESSOR)
                {
                    string deviceName = pSDK.GetDeviceName(idxDevice);
                    int deviceIndex = idxDevice;
                    uint deviceClass = CPUIDSDK.CLASS_DEVICE_PROCESSOR;

                    var cpu = new CPUProvider(deviceName, deviceIndex, deviceClass);

                    arr.Add(cpu);
                }
            }

            return arr.ToArray();
        }

        private static IDeviceProvider[] GetGPUProviders()
        {
            int gpu_count = pSDK.GetNumberOfDisplayAdapter();

            var arr = new List<IDeviceProvider>(gpu_count);

            for (int idxDevice = 0; idxDevice < pSDK.GetNumberOfDevices(); idxDevice++)
            {
                if (pSDK.GetDeviceClass(idxDevice) == CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER)
                {
                    string deviceName = pSDK.GetDeviceName(idxDevice);
                    int deviceIndex = idxDevice;
                    uint deviceClass = CPUIDSDK.CLASS_DEVICE_PROCESSOR;

                    var gpu = new GPUProvider(deviceName, deviceIndex, deviceClass);

                    arr.Add(gpu);
                }
            }

            return arr.ToArray();
        }
    }
}
