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
        public string Name => "/help";

        public string Description => "List of available commands.";

        public event EventHandler<CommandResult> Completed;

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var commandsStr = new StringBuilder();

            foreach (ICommand command in cmdInfo.Commands)
            {
                commandsStr.AppendLine(command.ToString());
            }

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = commandsStr.ToString(),
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);
        }

        public void ExecuteAsync(object parameter)
        {
            Task.Run(() => Execute(parameter));
        }

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
