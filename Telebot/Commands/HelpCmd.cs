using System;
using System.Text;
using System.Threading.Tasks;
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

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            var commandsStr = new StringBuilder();

            var commands = Program.CmdFactory.GetAllEntities();

            foreach (ICommand command in commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = commandsStr.ToString()
            };

            await cbResult(result);
        }
    }
}
