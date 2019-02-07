using System;
using System.Threading.Tasks;
using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOnCmd : ICommand
    {
        public string Name => "/tempmon on";

        public string Description => "Turn on temperature monitoring.";

        public event EventHandler<CommandResult> Completed;

        private readonly ISettings settings;
        private readonly ITemperatureMonitor tempMon;

        public TempMonOnCmd()
        {
            settings = Program.container.GetInstance<ISettings>();
            tempMon = Program.container.GetInstance<ITemperatureMonitor>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            tempMon.Start();
            settings.MonitorEnabled = true;

            var info = new CommandResult
            {
                Message = cmdInfo.Message,
                Text = "Temperature monitor is turned on.",
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
