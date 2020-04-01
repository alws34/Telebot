using Common.Models;
using System.Threading.Tasks;
using Common.Contracts;
using Updater;

namespace Plugins.Update
{

    public class UpdatePlugin : IModule
    {
        private IAppExit appExit;
        private IAppUpdate appUpdate;

        public UpdatePlugin()
        {
            Pattern = "/update (chk|dl)";
            Description = "Check or download an update.";
        }

        public override async void Execute(Request req)
        {
            string state = req.Groups[1].Value;

            switch (state)
            {
                case "chk":
                    appUpdate.CheckUpdate();
                    break;
                case "dl":
                    var result = new Response(
                        "Telebot is updating...",
                        req.MessageId
                    );
                    await ResultHandler(result);
                    appUpdate.DownloadUpdate();
                    await Task.Delay(1500).ContinueWith(t => { appExit.Exit(); });
                    break;
            }
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);
            appUpdate = data.IocContainer.GetInstance<IAppUpdate>();
            appExit = data.IocContainer.GetInstance<IAppExit>();
        }
    }
}
