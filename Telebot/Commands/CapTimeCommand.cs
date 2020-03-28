using System;
using System.Threading.Tasks;
using Telebot.Capture;
using Telebot.Common;
using Telebot.Jobs;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapTimeCommand : ICommand
    {
        private readonly IJob<CaptureArgs> _job;

        public CapTimeCommand()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";
            OSVersion = new Version(5, 0);

            _job = Program.CaptureFactory.FindEntity(
                x => x.JobType == JobType.Scheduled
            );
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string arg = req.Groups[1].Value;

            if (arg.Equals("off"))
            {
                var result1 = new Response
                {
                    ResultType = ResultType.Text,
                    Text = "Successfully sent command \"off\" to screen capture."
                };

                await resp(result1);

                _job.Stop();

                return;
            }

            var intParams = arg.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            string text = $"Screen capture has been scheduled to run {duration} sec for every {interval} sec.";

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
