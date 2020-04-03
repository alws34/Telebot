using BotSdk.Contracts;
using BotSdk.Models;
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

namespace Telebot
{
    static class Program
    {
        public static Container IocContainer { get; private set; }

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
            IBotClient botClient = new TelebotClient(token, id);

            BuildContainer();
            InitializeModules(botClient.ResultHandler);

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

            var modules = LoadModules();

            var modulesType = IocContainer.GetTypesToRegister<IModule>(modules);
            var statusType = IocContainer.GetTypesToRegister<IJobStatus>(modules);

            IocContainer.Collection.Register<IModule>(modulesType.AsSingletons());
            IocContainer.Collection.Register<IJobStatus>(statusType.AsSingletons());

            DeviceCreator devCreator = new DeviceCreator();

            // Register all components
            IocContainer.Collection.Register(devCreator.GetAll());

            // Register each component individual
            IocContainer.Collection.Register(devCreator.GetProcessors());
            IocContainer.Collection.Register(devCreator.GetDisplays());
            IocContainer.Collection.Register(devCreator.GetDrives());
            IocContainer.Collection.Register(devCreator.GetMainboards());
            IocContainer.Collection.Register(devCreator.GetBatteries());

            IocContainer.Verify();
        }

        private static IEnumerable<Registration> AsSingletons(this IEnumerable<Type> types)
        {
            var result =
                from type in types
                select Lifestyle.Singleton.CreateRegistration(type, IocContainer);

            return result;
        }

        private static IEnumerable<Assembly> LoadModules()
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

        private static void InitializeModules(ResponseHandler handler)
        {
            var data = new ModuleData
            {
                IoCProvider = IocContainer,
                ResultHandler = handler
            };

            var modules = IocContainer.GetAllInstances<IModule>();

            foreach (IModule module in modules)
            {
                module.Initialize(data);
            }
        }
    }
}
