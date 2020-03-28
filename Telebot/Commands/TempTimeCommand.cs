using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Jobs;
using Telebot.Models;
using Telebot.Temperature;

namespace Telebot.Commands
{
    public class TempTimeCommand : ICommand
    {
        private readonly IJob<TempArgs> _job;

        public TempTimeCommand()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";
            OSVersion = new Version(5, 1);

            _job = Program.TempFactory.FindEntity(
                x => x.JobType == JobType.Scheduled
            );
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var arg = req.Groups[1].Value;

            if (arg.Equals("off"))
            {
                var result1 = new Response
                {
                    ResultType = ResultType.Text,
                    Text = "Successfully sent command \"off\" temperature monitor."
                };

                await resp(result1);

                _job.Stop();

                return;
            }

            var intParams = arg.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            string text = $"Temperature monitor has been scheduled to run {duration} sec for every {interval} sec.";

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = text
            };

            await resp(result);

            ((IScheduled)_job).Start(duration, interval);
        }
    }
}
