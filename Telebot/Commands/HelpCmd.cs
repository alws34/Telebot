using System;
using System.Text;
using Telebot.Commands.Factories;
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

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var commandsStr = new StringBuilder();

            var commands = Program.commandFactory.GetAllCommands();

            foreach (ICommand command in commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new CommandResult
            {
                Text = commandsStr.ToString(),
                SendType = SendType.Text
            };

            callback(result);
        }
    }
}
