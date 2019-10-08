using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Contracts;
using Telebot.Models;
using Telebot.Temperature;

namespace Telebot.Commands
{
    public class TempTimeCmd : CommandBase
    {
        private readonly IJob<TempChangedArgs> _job;

        public TempTimeCmd()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";

            _job = Program.tempMonFactory.FindEntity(
                x => x.JobType == JobType.Scheduled
            );
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
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

                await callback(cmdResult);

                _job.Stop();

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

            await callback(result);

            ((IScheduledJob)_job).Start(tsDuration, tsInterval);
        }
    }
}
