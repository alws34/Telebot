using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : ICommand
    {
        private readonly PowerImpl power;

        public ShutdownCmd()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";

            power = new PowerImpl();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int sec = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully scheduled the workstation to shutdown in {sec} seconds."
            };

            await resp(result);

            power.ShutdownWorkstation(sec);
        }
    }
}
