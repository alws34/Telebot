using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class HelpCmd : ICommand
    {
        public string Pattern => "/help";

        public string Description => "List of available commands.";

        public event EventHandler<CommandResult> Completed;

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var commandsStr = new StringBuilder();

            foreach (ICommand command in parameters.Commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = commandsStr.ToString(),
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);
        }

        public void ExecuteAsync(object parameter)
        {
            Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
