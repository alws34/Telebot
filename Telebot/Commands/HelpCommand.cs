using System;
using System.Text;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class HelpCommand : ICommand
    {
        public HelpCommand()
        {
            Pattern = "/help";
            Description = "List of available commands.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var commandsStr = new StringBuilder();

            var commands = Program.CommandFactory.GetAllEntities();

            foreach (ICommand command in commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = commandsStr.ToString()
            };

            await resp(result);
        }
    }
}
