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

            User32Helper.LockWorkStation();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Locked down the host machine.",
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
