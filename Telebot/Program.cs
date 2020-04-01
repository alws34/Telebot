using Common;
using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID;
using CPUID.Base;
using CPUID.Factories;
using FluentScheduler;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Telebot.Clients;
using Telebot.Presenters;
using Telebot.Settings;
using Updater;

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

            CpuIdWrapper64.Sdk64.UninitSDK();
        }

        private static void BuildContainer()
        {
            IocContainer = new Container();

            IAppExit appExit = new AppExit(Application.Exit);
            IAppRestart appRestart = new AppRestart(Application.Restart);

            IocContainer.RegisterInstance(typeof(IAppExit), appExit);
            IocContainer.RegisterInstance(typeof(IAppRestart), appRestart);

            IocContainer.Register<IAppUpdate, AppUpdate>(Lifestyle.Singleton);
            IocContainer.Register<IFactory<IDevice>, DeviceFactory>(Lifestyle.Singleton);

            var modules = LoadAssemblies();

            var modTypes = IocContainer.GetTypesToRegister<IPlugin>(modules);
            var sttTypes = IocContainer.GetTypesToRegister<IModuleStatus>(modules);

            var modReg =
                from type in modTypes
                select Lifestyle.Singleton.CreateRegistration(type, IocContainer);

            var sttReg =
                from type in sttTypes
                select Lifestyle.Singleton.CreateRegistration(type, IocContainer);

            IocContainer.Collection.Register<IPlugin>(modReg);
            IocContainer.Collection.Register<IModuleStatus>(sttReg);

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
}
