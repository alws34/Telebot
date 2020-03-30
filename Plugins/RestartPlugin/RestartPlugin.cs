using Contracts;
using Common.Models;
using System;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

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
            MinOSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var result = new Response("Telebot is restarting...");

            await resp(result);

            await Task.Delay(2000).ContinueWith((t) =>
            {
                Restart();
            });
        }

        public override void Initialize(PluginData data)
        {
            Restart = data.Restart;
        }
    }
}
