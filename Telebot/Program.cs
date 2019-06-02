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
using Telebot.Presenters;
using Telebot.Settings;
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

            var RefreshThread = new Thread(RefreshThreadProc);

            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            bool sdkLoaded = pSDK.InitSDK_Quick();

            if (!sdkLoaded)
            {
                throw new Exception("Couldn't load cpuidsdk module.");
            }

            RefreshThread.Start();

            buildContainer();

            logger = new FileLogger();
            appSettings = new SettingsManager();
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

        private static void buildContainer()
        {
            container = new Container();

            var providers = GetCPUProviders().Concat(GetGPUProviders());
            container.Collection.Register<IDeviceProvider>(providers);
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
                    uint deviceClass = CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER;

                    var gpu = new GPUProvider(deviceName, deviceIndex, deviceClass);

                    arr.Add(gpu);
                }
            }

            return arr.ToArray();
        }
    }
}
