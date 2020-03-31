using AutoUpdaterDotNET;
using Common;
using Contracts;
using Contracts.Factories;
using CPUID;
using CPUID.Base;
using CPUID.Factories;
using FluentScheduler;
using SimpleInjector;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Telebot.AppSettings;
using Telebot.Clients;
using Telebot.Plugins;
using Telebot.Presenters;

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

            GlobalSettings.CreateInstance();

            string token = GlobalSettings.Instance.Telegram.GetBotToken();
            int id = GlobalSettings.Instance.Telegram.GetAdminId();

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

            IocContainer = new Container();

            IocContainer.Register<IAppExit, AppExit>();
            IocContainer.Register<IAppRestart, AppRestart>();

            IocContainer.Register<IFactory<IDevice>, DeviceFactory>(Lifestyle.Singleton);
            IocContainer.Register<IFactory<IPlugin>, PluginFactory>(Lifestyle.Singleton);

            // Create registration instances
            IocContainer.Verify();

            var plugins = IocContainer.GetInstance<IFactory<IPlugin>>().GetAllEntities();

            foreach (IPlugin plugin in plugins)
            {
                plugin.Initialize(IocContainer);
            }

            AutoUpdater.AppCastURL = ConfigurationManager.AppSettings["updateUrl"];

            MainView mainView = new MainView();
            IBotClient botClient = new Clients.Telebot(token, id);

            var presenter = new MainViewPresenter(
                mainView,
                botClient
            );

            JobManager.AddJob(() =>
            {
                AutoUpdater.Start();
            }, s => s.WithName("CheckForUpdate").ToRunEvery(1).Hours()
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
    }
}
