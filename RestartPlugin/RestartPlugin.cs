using Contracts;
using FluentScheduler;
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

            JobManager.AddJob(() =>
            {
                // restart logic
            }, (s) => s.ToRunOnceIn(2).Seconds()
            );
        }
    }
}
