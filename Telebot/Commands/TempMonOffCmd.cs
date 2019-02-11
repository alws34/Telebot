using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOffCmd : CommandBase
    {
        private readonly ISettings settings;
        private readonly ITemperatureMonitor tempMon;

        public TempMonOffCmd()
        {
            Pattern = "/tempmon off";
            Description = "Turn off temperature monitoring.";
            settings = Program.container.GetInstance<ISettings>();
            tempMon = Program.container.GetInstance<ITemperatureMonitor>();
        }

        public override CommandResult Execute(object parameter)
        {
            tempMon.Stop();
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
