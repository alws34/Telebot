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
            MinOsVersion = new Version(5, 0);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            var result = new Response("Telebot is closing...");

            await resp(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Exit();
            });
        }

        public override void Initialize(Container iocContainer)
        {
            var instance = iocContainer.GetInstance<IAppExit>();

            Exit = instance.Exit();
        }
    }
}
