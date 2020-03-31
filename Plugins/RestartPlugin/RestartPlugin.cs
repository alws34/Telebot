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

            await respHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Restart();
            });
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            var instance = iocContainer.GetInstance<IAppRestart>();

            Restart = instance.Restart();
        }
    }
}
