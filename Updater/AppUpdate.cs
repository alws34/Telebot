using AutoUpdaterDotNET;
using FluentScheduler;
using System.Configuration;

namespace Updater
{
    public interface IAppUpdate
    {
        void CheckUpdate();
        void DownloadUpdate();

        event AutoUpdater.CheckForUpdateEventHandler HandleCheck;
    }

    public class AppUpdate : IAppUpdate
    {
        public event AutoUpdater.CheckForUpdateEventHandler HandleCheck
        {
            add => AutoUpdater.CheckForUpdateEvent += value;
            remove => AutoUpdater.CheckForUpdateEvent -= value;
        }

        public AppUpdate()
        {
            AutoUpdater.AppCastURL = ConfigurationManager.AppSettings["updateUrl"];

            JobManager.AddJob(() =>
                {
                    CheckUpdate();
                }, s => s.WithName("CheckForUpdate").ToRunNow().AndEvery(1).Hours()
            );
        }

        public void CheckUpdate()
        {
            AutoUpdater.Start();
        }

        public void DownloadUpdate()
        {
            AutoUpdater.DownloadUpdate();
        }
    }
}
