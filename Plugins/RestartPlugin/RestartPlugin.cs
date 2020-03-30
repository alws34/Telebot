using Contracts;
using Models;
using System;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class RestartCommand : IPlugin
    {
        private Action Restart;

        public RestartCommand()
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

        public override void Initialize(IPluginData data)
        {
            Restart = data.Restart;
        }
    }
}
