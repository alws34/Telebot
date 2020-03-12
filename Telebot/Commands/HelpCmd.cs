using System;
using System.Text;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class HelpCmd : ICommand
    {
        public HelpCmd()
        {
            Pattern = "/help";
            Description = "List of available commands.";
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            var commandsStr = new StringBuilder();

            var commands = Program.CmdFactory.GetAllEntities();

            foreach (ICommand command in commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = commandsStr.ToString()
            };

            await cbResult(result);
        }
    }
}
