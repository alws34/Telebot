using System.Text;
using Telebot.Models;

namespace Telebot.Commands
{
    public class HelpCmd : CommandBase
    {
        public HelpCmd()
        {
            Pattern = "/help";
            Description = "List of available commands.";
        }

        public override CommandResult Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var commandsStr = new StringBuilder();

            foreach (ICommand command in parameters.Commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new CommandResult
            {
                Text = commandsStr.ToString(),
                SendType = SendType.Text
            };

            return result;
        }
    }
}
