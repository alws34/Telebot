using System;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.Helpers;

namespace Telebot.Commands
{
    public class LockCmd : ICommand
    {
        public string Name => "/lock";

        public string Description => "Locks the workstation.";

        public event EventHandler<CommandResult> Completed;

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Locked down the workstation.",
                SendType = SendType.Text
            };

            Completed?.Invoke(this, info);

            User32Helper.LockWorkStation();
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
