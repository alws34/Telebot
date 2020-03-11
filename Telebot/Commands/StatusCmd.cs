using System;
using System.Text;
using System.Threading.Tasks;
using Telebot.Commands.Status;
using Telebot.Models;

namespace Telebot.Commands
{
    public class StatusCmd : CommandBase
    {
        private readonly IStatus[] statuses;

        public StatusCmd(IStatus[] statuses)
        {
            Pattern = "/status";
            Description = "Receive workstation information.";

            this.statuses = statuses;
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.Execute());
            }

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = statusBuilder.ToString()
            };

            await cbResult(result);
        }
    }
}
