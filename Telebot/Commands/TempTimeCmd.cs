using System;
using System.Collections.Generic;
using System.Text;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempTimeCmd : CommandBase
    {
        public TempTimeCmd()
        {
            Pattern = "/temptime -d (\\d+) -i (\\d+)";
            Description = "Schedules a temperature monitor.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            void temperatureChanged(IEnumerable<HardwareInfo> devices)
            {
                var text = new StringBuilder();

                foreach (HardwareInfo device in devices)
                {
                    text.AppendLine($"*{device.DeviceName}*: {device.Value}°C");
                }

                text.AppendLine("\nFrom *Telebot*");

                var res = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = text.ToString()
                };

                callback(res);
            }

            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Groups[2].Value);

            TimeSpan tsDuration = TimeSpan.FromSeconds(duration);
            TimeSpan tsInterval = TimeSpan.FromSeconds(interval);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled temperature monitor."
            };

            callback(result);

            ScheduledTempMonitor.Instance.Start(tsDuration, tsInterval, temperatureChanged);
        }
    }
}
