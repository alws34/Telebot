using System.Collections.Generic;
using System.Text;
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
            statusCommands = Program.container.GetAllInstances<IStatusCommand>();
        }

        public override CommandResult Execute(object parameter)
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

            return result;
        }
    }
}
