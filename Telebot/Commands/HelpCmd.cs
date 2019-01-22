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

        private readonly Form1 form1;

        public HelpCmd()
        {
            form1 = Program.container.GetInstance<Form1>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var builder = new StringBuilder();

            foreach (ICommand command in form1.Commands.Values)
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
