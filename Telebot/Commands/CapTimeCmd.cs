using System;
using Telebot.Contracts;
using Telebot.Models;
using Telebot.ScreenCapture;

namespace Telebot.Commands
{
    public class CapTimeCmd : CommandBase
    {
        private readonly IScreenCapture screenCapture;

        public CapTimeCmd()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";

            screenCapture = Program.screenCapFactory.FindEntity(
                x => x.ScreenCapType == ScreenCapType.Scheduled
            );
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            var reParam = parameters.Groups[1].Value;

            if (reParam.Equals("off"))
            {
                var cmdResult = new CommandResult
                {
                    SendType = SendType.Text,
                    Text = "Successfully disabled scheduled screen capture."
                };

                callback(cmdResult);

                screenCapture.Stop();

                return;
            }

            var intParams = reParam.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            TimeSpan tsDuration = TimeSpan.FromSeconds(duration);
            TimeSpan tsInterval = TimeSpan.FromSeconds(interval);

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully scheduled screen capture."
            };

            callback(result);

            ((IScheduledJob)screenCapture).Start(tsDuration, tsInterval);
        }
    }
}
