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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var commandsStr = new StringBuilder();

            var commands = Program.CmdFactory.GetAllEntities();

            foreach (ICommand command in commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new CommandResult
            {
                Text = commandsStr.ToString(),
                SendType = SendType.Text
            };

            await callback(result);
        }
    }
}
