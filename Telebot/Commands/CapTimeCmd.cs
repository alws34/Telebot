using System;
using Telebot.Models;
using Telebot.ScreenCaptures;

namespace Telebot.Commands
{
    public class CapTimeCmd : CommandBase
    {
        public CapTimeCmd()
        {
            Pattern = "/captime (\\d+) (\\d+)";
            Description = "Schedules screen capture session.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Groups[2].Value);

            TimeSpan tsDuration = TimeSpan.FromSeconds(duration);
            TimeSpan tsInterval = TimeSpan.FromSeconds(interval);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled screen capture."
            };

            callback(result);

            ScheduledScreenCapture.Instance.Start(tsDuration, tsInterval);
        }
    }
}
