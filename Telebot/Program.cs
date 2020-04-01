using Common.Contracts;
using Common.Models;
using CPUID.Base;
using CPUID.Devices;
using FluentScheduler;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Presenters;
using Telebot.Settings;
using Updater;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace Telebot
{
    static class Program
    {
        public static Container IocContainer { get; private set; }

        private static IBotClient botClient;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IAppSettings appSettings = new AppSettings();

            string token = appSettings.Telegram.GetBotToken();
            int id = appSettings.Telegram.GetAdminId();

            if (string.IsNullOrEmpty(token) || id == 0)
            {
                MessageBox.Show(
                    "Missing Token or AdminId in settings.ini.",
                    "Telebot",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            MainView mainView = new MainView();
            botClient = new TelebotClient(token, id);

            BuildContainer();
            InitializeModules();

            var presenter = new MainViewPresenter(
                mainView,
                botClient
            );

            Application.Run(mainView);

            int jobsCount = JobManager.AllSchedules.Count();

            if (jobsCount > 0)
            {
                // todo - save ongoing jobs to disk and reload them?

                JobManager.StopAndBlock();
                JobManager.RemoveAllJobs();
            }

            Sdk64.UninitSDK();
        }

        private static void BuildContainer()
        {
            IocContainer = new Container();

            IAppExit appExit = new AppExit(Application.Exit);
            IAppRestart appRestart = new AppRestart(Application.Restart);

            IocContainer.RegisterInstance(typeof(IAppExit), appExit);
            IocContainer.RegisterInstance(typeof(IAppRestart), appRestart);

            IocContainer.Register<IAppUpdate, AppUpdate>(Lifestyle.Singleton);

            var assemblies = LoadAssemblies();

            var modulesType = IocContainer.GetTypesToRegister<IPlugin>(assemblies);
            var statusType = IocContainer.GetTypesToRegister<IModuleStatus>(assemblies);

            var modulesRegistration =
                from type in modulesType
                select Lifestyle.Singleton.CreateRegistration(type, IocContainer);

            var statusRegistration =
                from type in statusType
                select Lifestyle.Singleton.CreateRegistration(type, IocContainer);

            IocContainer.Collection.Register<IPlugin>(modulesRegistration);
            IocContainer.Collection.Register<IModuleStatus>(statusRegistration);

            DevicesRegistration devicesRegistration = new DevicesRegistration();
            var devices = devicesRegistration.GetDevices();

            IocContainer.Collection.Register<IDevice>(devices);

            IocContainer.Verify();
        }

        private static IEnumerable<Assembly> LoadAssemblies()
        {
            var assemblies = Directory.EnumerateFiles(
                ".\\Plugins", "*Plugin.dll", SearchOption.AllDirectories
            );

            var modules = new List<Assembly>();

            foreach (string assemblyName in assemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyName);
                modules.Add(assembly);
            }

            return modules;
        }

        private static void InitializeModules()
        {
            var data = new PluginData
            {
                IocContainer = IocContainer,
                ResultHandler = botClient.ResultHandler
            };

            var modules = IocContainer.GetAllInstances<IPlugin>();

            foreach (IPlugin module in modules)
            {
                module.Initialize(data);
            }
        }
    }

    public class DevicesRegistration
    {
        private readonly int deviceCount;

        public DevicesRegistration()
        {
            deviceCount = Sdk64.GetNumberOfDevices();
        }

        public IEnumerable<IDevice> GetDevices()
        {
            var cpuItems = LoadDevices<CPUDevice>(CLASS_DEVICE_PROCESSOR);
            var gpuItems = LoadDevices<GPUDevice>(CLASS_DEVICE_DISPLAY_ADAPTER);
            var ramItems = LoadDevices<RAMDevice>(CLASS_DEVICE_MAINBOARD);
            var hddItems = LoadDevices<HDDDevice>(CLASS_DEVICE_DRIVE);
            var batItems = LoadDevices<BATDevice>(CLASS_DEVICE_BATTERY);

            var result = new List<IDevice>();
            result.AddRange(cpuItems);
            result.AddRange(gpuItems);
            result.AddRange(ramItems);
            result.AddRange(hddItems);
            result.AddRange(batItems);

            return result;
        }

        private IEnumerable<T> LoadDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var items = new List<T>();

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (Sdk64.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = Sdk64.GetDeviceName(deviceIndex);

                    T device = (T)Activator.CreateInstance(typeof(T), new object[]
                    {
                        deviceName,
                        deviceIndex,
                        deviceClass
                    });

                    items.Add(device);
                }
            }

            return items;
        }
    }
}
