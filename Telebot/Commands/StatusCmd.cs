using System;
using System.Text;
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

        public override void Execute(object parameter, Action<CommandResult> callback)
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

            callback(result);
        }
    }
}
