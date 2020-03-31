using Common;
using Common.Models;
using Contracts;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Plugins.Exit
{
    [Export(typeof(IPlugin))]
    public class ExitPlugin : IPlugin
    {
        private IAppExit appExit;

        public ExitPlugin()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response("Telebot is closing...", req.MessageId);

            await resultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                appExit.Exit();
            });
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);
            appExit = data.iocContainer.GetInstance<IAppExit>();
        }
    }
}
