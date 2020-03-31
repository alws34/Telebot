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
using System.Linq;
using System.Windows.Forms;
using Telebot.AppSettings;
using Telebot.Clients;
using Telebot.Plugins;
using Telebot.Presenters;
using Updater;

namespace Telebot
{
    static class Program
    {
        public static Container IocContainer { get; private set; }
        public static IAppUpdate AppUpdate { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IAppSettings appSettings = new AppSettings.AppSettings();

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

            BuildIocContainer();

            AppUpdate = IocContainer.GetInstance<IAppUpdate>();

            MainView mainView = new MainView();
            IBotClient botClient = new Clients.Telebot(token, id);

            var mgr = IocContainer.GetInstance<IFactory<IPlugin>>();

            var data = new PluginData
            {
                ResultHandler = botClient.ResultHandler,
                iocContainer = IocContainer
            };

            (mgr as PluginFactory)?.InitializePlugins(data);

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

        private static void BuildIocContainer()
        {
            IocContainer = new Container();

            IocContainer.Register<IAppExit, AppExit>();
            IocContainer.Register<IAppRestart, AppRestart>();

            IocContainer.Register<IAppUpdate, AppUpdate>(Lifestyle.Singleton);
            IocContainer.Register<IFactory<IDevice>, DeviceFactory>(Lifestyle.Singleton);
            IocContainer.Register<IFactory<IPlugin>, PluginFactory>(Lifestyle.Singleton);

            // Create registration instances
            IocContainer.Verify();
        }
    }
}
