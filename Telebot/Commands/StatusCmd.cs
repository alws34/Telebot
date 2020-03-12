using System;
using System.Text;
using System.Threading.Tasks;
using Telebot.Commands.Status;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class StatusCmd : ICommand
    {
        private readonly IStatus[] statuses;

        public StatusCmd(IStatus[] statuses)
        {
            Pattern = "/status";
            Description = "Receive workstation information.";

            this.statuses = statuses;
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.GetStatus());
            }

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = statusBuilder.ToString()
            };

            await cbResult(result);
        }
    }
}
