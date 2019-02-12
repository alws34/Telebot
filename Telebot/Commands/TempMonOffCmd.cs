using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.Monitors.Factories;

namespace Telebot.Commands
{
    public class TempMonOffCmd : CommandBase
    {
        private readonly ISettings settings;
        private readonly ITemperatureMonitor temperatureMonitor;

        public TempMonOffCmd()
        {
            Pattern = "/tempmon off";
            Description = "Turn off temperature monitoring.";
            settings = Program.container.GetInstance<ISettings>();
            temperatureMonitor = TempMonitorFactory.Instance.GetTemperatureMonitor<PermanentTempMonitor>();
        }

        public override CommandResult Execute(object parameter)
        {
            temperatureMonitor.Stop();
            settings.TempMonEnabled = false;

            var result = new CommandResult
            {
                Text = "Temperature monitor is turned off.",
                SendType = SendType.Text
            };

            return result;
        }
    }
}
