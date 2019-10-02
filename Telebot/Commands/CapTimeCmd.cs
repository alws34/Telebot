using System;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapTimeCmd : CommandBase
    {
        public CapTimeCmd()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";
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

                Program.screenCapture.Stop();

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

            ((IScheduledJob)Program.screenCapture).Start(tsDuration, tsInterval);
        }
    }
}
