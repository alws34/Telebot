using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCommand : ICommand
    {
        public ShutdownCommand()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";
            OSVersion = new Version(5, 1);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int timeout = Convert.ToInt32(req.Groups[1].Value);

            string text = $"Successfully scheduled the workstation to shutdown in {timeout} seconds.";

            var result = new Response(text);

            await resp(result);

            IApi api = new PowerApi(PowerType.Shutdown, timeout);

            api.Invoke();
        }
    }
}
