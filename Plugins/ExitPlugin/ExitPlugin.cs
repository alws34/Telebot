using Common;
using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Plugins.Exit
{
    [Export(typeof(IPlugin))]
    public class ExitPlugin : IPlugin
    {
        private Action Exit;

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
                Exit();
            });
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            var instance = data.iocContainer.GetInstance<IAppExit>();

            Exit = instance.Exit();
        }
    }
}
