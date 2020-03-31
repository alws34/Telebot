using Common;
using Common.Models;
using Contracts;
using SimpleInjector;
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
            var result = new Response("Telebot is closing...");

            await respHandler(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Exit();
            });
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            var instance = iocContainer.GetInstance<IAppExit>();

            Exit = instance.Exit();
        }
    }
}
