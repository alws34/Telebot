using Telebot.Managers;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOnCmd : CommandBase
    {
        private readonly ISettings settings;
        private readonly ITemperatureMonitor tempMon;

        public TempMonOnCmd()
        {
            Pattern = "/tempmon on";
            Description = "Turn on temperature monitoring.";
            settings = Program.container.GetInstance<ISettings>();
            tempMon = Program.container.GetInstance<ITemperatureMonitor>();
        }

        public override CommandResult Execute(object parameter)
        {
            tempMon.Start();
            settings.TempMonEnabled = true;

            var result = new CommandResult
            {
                Text = "Temperature monitor is turned on.",
                SendType = SendType.Text
            };

            return result;
        }
    }
}
