using Common;
using Common.Models;
using Contracts;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Plugins.Restart
{
    [Export(typeof(IPlugin))]
    public class RestartPlugin : IPlugin
    {
        private IAppRestart appRestart;

        public RestartPlugin()
        {
            Pattern = "/restart";
            Description = "Restart Telebot.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response(
                "Telebot is restarting...",
                req.MessageId
            );

            await resultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                appRestart.Restart();
            });
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);
            appRestart = data.iocContainer.GetInstance<IAppRestart>();
        }
    }
}
