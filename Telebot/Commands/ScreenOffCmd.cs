using System;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Helpers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOffCmd : ICommand
    {
        public string Name => "/screen off";

        public string Description => "Turn off the monitor.";

        public event EventHandler<CommandResult> Completed;

        private readonly ScreenController screenController;

        public ScreenOffCmd()
        {
            screenController = Program.container.GetInstance<ScreenController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenController.SetMonitorOff();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Display will be turned off now.",
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
