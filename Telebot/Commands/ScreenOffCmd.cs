using System;
using Telebot.Contracts;
using Telebot.BusinessLogic;
using Telebot.Models;
using System.Threading.Tasks;

namespace Telebot.Commands
{
    public class ScreenOffCmd : ICommand
    {
        public string Name => "/screen off";

        public string Description => "Turn off the monitor.";

        public event EventHandler<CommandResult> Completed;

        private readonly ScreenLogic screenLogic;

        public ScreenOffCmd()
        {
            screenLogic = Program.container.GetInstance<ScreenLogic>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            screenLogic.SetMonitorOff();

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Display will be turned off now.",
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
