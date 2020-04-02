using BotSdk.Contracts;
using BotSdk.Extensions;
using BotSdk.Jobs;
using BotSdk.Models;
using System;
using Telebot.Capture;

namespace Plugins.CapTime
{
    public class CapTimePlugin : IModule, IJobStatus
    {
        private IJob<CaptureArgs> job;

        public CapTimePlugin()
        {
            Pattern = "/captime (off|(\\d+) (\\d+))";
            Description = "Schedules screen capture session.";
        }

        public override async void Execute(Request req)
        {
            string state = req.Groups[1].Value;

            if (state.Equals("off"))
            {
                var result1 = new Response(
                    $"Screen capture has turned {state}.",
                    req.MessageId
                );

                await ResultHandler(result1);
                job.Stop();
                return;
            }

            var args = state.Split(' ');

            int duration = Convert.ToInt32(args[0]);
            int interval = Convert.ToInt32(args[1]);

            string text = $"Screen capture has been scheduled to run {duration} sec for every {interval} sec.";

            var resp = new Response(text, req.MessageId);

            await ResultHandler(resp);

            ((IScheduled)job).Start(duration, interval);
        }

        private async void UpdateHandler(object sender, CaptureArgs e)
        {
            var update = new Response(e.Capture.ToMemStream());
            await ResultHandler(update);
        }

        private async void FeedbackHandler(object sender, Feedback e)
        {
            var result = new Response(e.Text);
            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            job = new CaptureSchedule
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }

        public string GetStatus()
        {
            return $"*Cap Time*: {job.Active.ToReadable()}";
        }
    }
}
