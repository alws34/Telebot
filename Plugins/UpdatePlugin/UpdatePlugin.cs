using Common;
using Common.Models;
using Contracts;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Updater;

namespace Plugins.Update
{
    [Export(typeof(IPlugin))]
    public class UpdatePlugin : IPlugin
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

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);
            appUpdate = data.iocContainer.GetInstance<IAppUpdate>();
            appExit = data.iocContainer.GetInstance<IAppExit>();
        }
    }
}
