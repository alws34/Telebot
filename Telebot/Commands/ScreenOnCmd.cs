using System;
using Telebot.BusinessLogic;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenOnCmd : ICommand
    {
        public string Name => "/screen on";

        public string Description => "Turn on the monitor.";

        public event EventHandler<CommandResult> Completed;

        private readonly ScreenLogic screenLogic;

        public ScreenOnCmd()
        {
            screenLogic = Program.container.GetInstance<ScreenLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenLogic.SetMonitorOn();

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
