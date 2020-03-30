﻿using Contracts;
using Extensions;
using Models;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Telebot.Capture;

namespace CapTimePlugin
{
    [Export(typeof(IPlugin))]
    public class CapTimePlugin : IPlugin
    {
        private readonly IJob<CaptureArgs> _job;

        public CapTimePlugin()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";
            MinOSVersion = new Version(5, 0);

            _job = new CaptureSchedule();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            _job.Update = async (s, e) =>
            {
                var update = new Response(e.Capture.ToMemStream());

                await resp(update);
            };

            string arg = req.Groups[1].Value;

            if (arg.Equals("off"))
            {
                var result1 = new Response("Successfully sent command \"off\" to screen capture.");

                await resp(result1);

                _job.Stop();

                return;
            }

            var intParams = arg.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            string text = $"Screen capture has been scheduled to run {duration} sec for every {interval} sec.";

            var result = new Response(text);

            await resp(result);

            ((IScheduled)_job).Start(duration, interval);
        }

        public override bool GetJobActive()
        {
            return _job.Active;
        }

        public override string GetJobName()
        {
            return "Cap Time";
        }
    }
}
