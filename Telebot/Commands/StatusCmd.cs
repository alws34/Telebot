using System;
using System.Collections.Generic;
using System.Text;
using Telebot.Commands.StatusCommands;
using Telebot.Models;
using Telebot.StatusCommands;

namespace Telebot.Commands
{
    public class StatusCmd : CommandBase
    {
        private readonly IEnumerable<IStatusCommand> statuses;

        public StatusCmd()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";

            statuses = new IStatusCommand[]
            {
                new SystemCmd(),
                new IPCmd(),
                new UptimeCmd(),
                new TempMonitorCmd()
            };
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var statusBuilder = new StringBuilder();

            foreach (IStatusCommand status in statuses)
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
