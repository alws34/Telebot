using System;
using System.Text;
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

            var builder = new StringBuilder();

            foreach (ICommand command in cmdInfo.Form1.commands.Values)
            {
                builder.AppendLine(command.ToString());
            }

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = builder.ToString(),
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);
        }

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
