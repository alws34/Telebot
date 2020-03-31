using AutoUpdaterDotNET;

namespace Updater
{
    public interface IAppUpdate
    {
        void CheckUpdate();
        void DownloadUpdate();

        event AutoUpdater.CheckForUpdateEventHandler HandleCheck;
    }
}
