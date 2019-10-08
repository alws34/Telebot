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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatus status in statuses)
            {
                statusBuilder.AppendLine(status.Execute());
            }

            var result = new CommandResult
            {
                Text = statusBuilder.ToString(),
                SendType = SendType.Text
            };

            await callback(result);
        }
    }
}
