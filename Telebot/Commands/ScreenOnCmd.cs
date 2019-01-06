using System;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Helpers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOnCmd : ICommand
    {
        public string Name => "/screen on";

        public string Description => "Turn on the monitor.";

        public event EventHandler<CommandResult> Completed;

        private readonly ScreenController screenController;

        public ScreenOnCmd()
        {
            screenController = Program.container.GetInstance<ScreenController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenController.SetMonitorOn();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Display will be turned on now.",
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
