using Contracts;
using Models;
using System;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class RestartCommand : IPlugin
    {
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
                entity.Restart();
            });
        }

        public override void SetAppEntity(IAppEntity entity)
        {
            this.entity = entity;
        }
    }
}
