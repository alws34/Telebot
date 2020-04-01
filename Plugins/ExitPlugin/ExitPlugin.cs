using Common;
using Common.Models;
using Contracts;
using System.Threading.Tasks;

namespace Plugins.Exit
{

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

            await ResultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                appExit.Exit();
            });
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);
            appExit = data.IocContainer.GetInstance<IAppExit>();
        }
    }
}
