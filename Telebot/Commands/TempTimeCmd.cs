using System;
using Telebot.Models;
using Telebot.Monitors;
using Telebot.Monitors.Factories;

namespace Telebot.Commands
{
    public class TempTimeCmd : CommandBase
    {
        private readonly ITemperatureMonitor temperatureMonitor;

        public TempTimeCmd()
        {
            Pattern = "/temptime -d (\\d+) -i (\\d+)";
            Description = "Schedules a temperature monitor.";
            temperatureMonitor = TempMonitorFactory.Instance.GetTemperatureMonitor<ScheduledTempMonitor>();
        }

        public override CommandResult Execute(object parameter)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Groups[2].Value);

            TimeSpan tsDuration = TimeSpan.FromSeconds(duration);
            TimeSpan tsInterval = TimeSpan.FromSeconds(interval);

            temperatureMonitor.Start(tsDuration, tsInterval);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled temperature monitor."
            };

            return result;
        }
    }
}
