using System;
using Telebot.Contracts;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class RebootCmd : ICommand
    {
        public string Name => "/reboot";

        public string Description => "Reboots the host machine.";

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

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
