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
        private readonly IEnumerable<IStatusCommand> statusCommands;

        public StatusCmd()
        {
            Pattern = "/status";
            Description = "Receive workstation information.";

            statusCommands = new IStatusCommand[]
            {
                new SystemCmd(),
                new IPCmd(),
                new UptimeCmd(),
                new TempMonitorCmd()
            };
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var status = new StringBuilder();

            foreach (IStatusCommand cmd in statusCommands)
            {
                status.AppendLine(cmd.Execute());
            }

            var result = new CommandResult
            {
                Text = status.ToString(),
                SendType = SendType.Text
            };

            callback(result);
        }
    }
}
