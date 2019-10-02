using System;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class TempTimeCmd : CommandBase
    {
        public TempTimeCmd()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";
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
                    Text = "Successfully disabled scheduled temperature monitor."
                };

                callback(cmdResult);

                Program.tempMonSchedule.Stop();

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
                Text = "Successfully scheduled temperature monitor."
            };

            callback(result);

            ((IScheduledJob)Program.tempMonSchedule).Start(tsDuration, tsInterval);
        }
    }
}
