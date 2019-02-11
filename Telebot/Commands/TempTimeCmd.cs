using System;
using Telebot.Models;
using Telebot.ScheduledOperations;

namespace Telebot.Commands
{
    public class TempTimeCmd : CommandBase
    {
        private readonly IScheduledTemperatureMonitor scheduledTemperatureMonitor;

        public TempTimeCmd()
        {
            Pattern = "/temptime -d (\\d+) -i (\\d+)";
            Description = "Schedules a temperature monitor.";
            scheduledTemperatureMonitor = Program.container.GetInstance<IScheduledTemperatureMonitor>();
        }

        public override CommandResult Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Groups[2].Value);

            scheduledTemperatureMonitor.Start(duration, interval);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled temperature monitor."
            };

            return result;
        }
    }
}
