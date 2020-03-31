using Common;
using Common.Models;
using Contracts;
using SimpleInjector;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Plugins.Restart
{
    [Export(typeof(IPlugin))]
    public class RestartPlugin : IPlugin
    {
        private Action Restart;

        public RestartPlugin()
        {
            Pattern = "/restart";
            Description = "Restart Telebot.";
        }

        public override async void Execute(Request req)
        {
            var result = new Response("Telebot is restarting...");

            await resultHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Restart();
            });
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            var instance = data.iocContainer.GetInstance<IAppRestart>();

            Restart = instance.Restart();
        }
    }
}
