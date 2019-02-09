using System;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.Helpers;

namespace Telebot.Commands
{
    public class LockCmd : ICommand
    {
        public string Pattern => "/lock";

        public string Description => "Locks the workstation.";

        public event EventHandler<CommandResult> Completed;

        public void Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            var result = new CommandResult
            {
                Message = parameters.Message,
                Text = "Locked down the workstation.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, result);

            User32Helper.LockWorkStation();
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
