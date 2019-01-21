using System;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class RebootCmd : ICommand
    {
        public string Name => "/reboot";

        public string Description => "Reboots the host machine.";

        private readonly PowerController powerController;

        public event EventHandler<CommandResult> Completed;

        public RebootCmd()
        {
            powerController = Program.container.GetInstance<PowerController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            powerController.RestartWindows();

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
