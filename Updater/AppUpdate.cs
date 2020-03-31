﻿using AutoUpdaterDotNET;
using FluentScheduler;
using System.Configuration;

namespace Updater
{
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
                }, s => s.WithName("CheckForUpdate").ToRunEvery(1).Hours()
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
