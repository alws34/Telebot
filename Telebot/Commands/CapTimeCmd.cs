﻿using System;
using System.Threading.Tasks;
using Telebot.Capture;
using Telebot.Common;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapTimeCmd : ICommand
    {
        private readonly IJob<CaptureArgs> _job;

        public CapTimeCmd()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";

            _job = Program.ScreenFactory.FindEntity(
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
                    Text = "Successfully disabled scheduled screen capture."
                };

                await resp(result1);

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
                Text = "Successfully scheduled screen capture."
            };

            await resp(result);

            ((IScheduled)_job).Start(tsDuration, tsInterval);
        }
    }
}
