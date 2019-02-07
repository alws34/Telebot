using System;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class RebootCmd : ICommand
    {
        public string Name => "/reboot";

        public string Description => "Reboots the workstation.";

        private readonly PowerLogic powerLogic;

        public event EventHandler<CommandResult> Completed;

        public RebootCmd()
        {
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            powerLogic.RestartWindows();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Restarting the host machine...",
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
