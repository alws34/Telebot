using Common;
using Common.Models;
using Contracts;
using Contracts.Factories;
using CPUID;
using CPUID.Base;
using CPUID.Factories;
using FluentScheduler;
using Shared;
using SimpleInjector;
using System;
using System.Linq;
using System.Windows.Forms;
using Telebot.AppSettings;
using Telebot.Clients;
using Telebot.Presenters;
using Updater;

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

            MainView mainView = new MainView();
            IBotClient botClient = new TeleBot(token, id);

            InitializePlugins(botClient);

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

        private static void InitializePlugins(IBotClient botClient)
        {
            var pluginFac = IocContainer.GetInstance<IFactory<IPlugin>>();

            var data = new PluginData
            {
                ResultHandler = botClient.ResultHandler,
                iocContainer = IocContainer
            };

            (pluginFac as IInitializer)?.Initialize(data);
        }

        private static void BuildIocContainer()
        {
            IocContainer = new Container();

            IAppExit appExit = new AppExit(Application.Exit);
            IAppRestart appRestart = new AppRestart(Application.Restart);

            IocContainer.RegisterInstance(typeof(IAppExit), appExit);
            IocContainer.RegisterInstance(typeof(IAppRestart), appRestart);

            IocContainer.Register<IAppUpdate, AppUpdate>(Lifestyle.Singleton);
            IocContainer.Register<IFactory<IDevice>, DeviceFactory>(Lifestyle.Singleton);

            ILoader loader = new ModuleLoader();

            IFactory<IPlugin> pluginFac = new PluginFactory(loader);
            IFactory<IStatus> statusFac = new StatusFactory(loader);

            IocContainer.RegisterInstance(typeof(IFactory<IPlugin>), pluginFac);
            IocContainer.RegisterInstance(typeof(IFactory<IStatus>), statusFac);

            IocContainer.Verify();
        }
    }
}
