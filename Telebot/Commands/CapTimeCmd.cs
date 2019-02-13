using System;
using Telebot.Models;
using Telebot.ScheduledOperations;

namespace Telebot.Commands
{
    public class CapTimeCmd : CommandBase
    {
        private readonly IScheduledScreenCapture scheduledScreenCapture;

        public CapTimeCmd()
        {
            Pattern = "/captime -d (\\d+) -i (\\d+)";
            Description = "Schedules a screen capture session.";
            scheduledScreenCapture = Program.container.GetInstance<IScheduledScreenCapture>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int duration = Convert.ToInt32(parameters.Groups[1].Value);
            int interval = Convert.ToInt32(parameters.Groups[2].Value);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled screen capture."
            };

            callback(result);

            scheduledScreenCapture.Start(duration, interval);
        }
    }
}
