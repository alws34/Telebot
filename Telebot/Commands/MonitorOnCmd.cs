using System;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class MonitorOnCmd : ICommand
    {
        public string Name => "/monitor on";

        public string Description => "Turn on monitoring of temperature.";

        public event EventHandler<CommandResult> Completed;

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            cmdInfo.Form1.tempMonitor.Start();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Temperature monitor is turned on.",
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
