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
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int timeout = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully scheduled the workstation to shutdown in {timeout} seconds."
            };

            await resp(result);

            IApi api = new PowerApi(PowerType.Shutdown, timeout);

            ApiInvoker.Instance.Invoke(api);
        }
    }
}
