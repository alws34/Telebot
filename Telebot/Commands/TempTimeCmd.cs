using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Contracts;
using Telebot.Models;
using Telebot.Temperature;

namespace Telebot.Commands
{
    public class TempTimeCmd : ICommand
    {
        private readonly IJob<TempChangedArgs> _job;

        public TempTimeCmd()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";

            _job = Program.TempFactory.FindEntity(
                x => x.JobType == JobType.Scheduled
            );
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            var arg = info.Groups[1].Value;

            if (arg.Equals("off"))
            {
                var result1 = new Response
                {
                    ResultType = ResultType.Text,
                    Text = "Successfully disabled scheduled temperature monitor."
                };

                await cbResult(result1);

                _job.Stop();

                return;
            }

            var intParams = arg.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            TimeSpan tsDuration = TimeSpan.FromSeconds(duration);
            TimeSpan tsInterval = TimeSpan.FromSeconds(interval);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = "Successfully scheduled temperature monitor."
            };

            await cbResult(result);

            ((IScheduledJob)_job).Start(tsDuration, tsInterval);
        }
    }
}
