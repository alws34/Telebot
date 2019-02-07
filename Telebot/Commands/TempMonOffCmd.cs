using System;
using System.Threading.Tasks;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOffCmd : ICommand
    {
        public string Name => "/tempmon off";

        public string Description => "Turn off temperature monitoring.";

        public event EventHandler<CommandResult> Completed;

        private readonly ISettings settings;
        private readonly ITemperatureMonitor tempMon;

        public TempMonOffCmd()
        {
            settings = Program.container.GetInstance<ISettings>();
            tempMon = Program.container.GetInstance<ITemperatureMonitor>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            tempMon.Stop();
            settings.MonitorEnabled = false;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Temperature monitor is turned off.",
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
