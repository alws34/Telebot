using AutoUpdaterDotNET;
using FluentScheduler;
using SimpleInjector;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Telebot.AppSettings;
using Telebot.Clients;
using Telebot.NSPlugins;
using Telebot.Presenters;
using static CPUID.CpuIdWrapper64;

namespace Telebot
{
    static class Program
    {
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

            AutoUpdater.AppCastURL = ConfigurationManager.AppSettings["updateUrl"];

            MainView mainView = new MainView();
            IBotClient botClient = new Clients.Telebot(token, id);

            var presenter = new MainViewPresenter(
                mainView,
                botClient
            );

            JobManager.AddJob(() =>
            {
                Sdk64.RefreshInformation();
            }, (s) => s.WithName("RefreshInformation").ToRunNow().AndEvery(1).Seconds()
            );

            JobManager.AddJob(() =>
            {
                AutoUpdater.Start();
            }, (s) => s.WithName("CheckForUpdate").ToRunEvery(1).Hours()
            );

            Plugins.CreateInstance();

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
    }
}
