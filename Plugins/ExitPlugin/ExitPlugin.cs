using Contracts;
using Models;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    [Export(typeof(IPlugin))]
    public class ExitPlugin : IPlugin
    {
        public ExitPlugin()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var result = new Response("Telebot is closing...");

            await resp(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Environment.Exit(0);
            });
        }
    }
}
