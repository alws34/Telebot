using AppSettings.Singletons;
using AutoUpdaterDotNET;
using FluentScheduler;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Clients;
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

            string token = GlobalSettings.Instance.Telegram.GetBotToken();
            int id = GlobalSettings.Instance.Telegram.GetAdminId();

            if (string.IsNullOrEmpty(token) || id == 0)
            {
                MessageBox.Show(
                    "Missing Token or AdminId in settings.ini.",
                    "Missing info",
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

            Application.ApplicationExit += ApplicationExit;

            Application.Run(mainView);
        }

        private static void ApplicationExit(object sender, EventArgs e)
        {
            int jobsCount = JobManager.AllSchedules.Count();

            if (jobsCount > 0)
            {
                // todo - save ongoing jobs to disk and reload them?

                JobManager.StopAndBlock();
                JobManager.RemoveAllJobs();
            }

            GlobalSettings.Instance.Main.CommitChanges();

            GlobalSettings.Instance.Main.WriteChanges();

            Sdk64.UninitSDK();
        }
    }
}
