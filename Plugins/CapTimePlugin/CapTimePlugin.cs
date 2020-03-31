using Common;
using Common.Extensions;
using Common.Models;
using Contracts;
using Contracts.Jobs;
using System;
using System.ComponentModel.Composition;
using Telebot.Capture;

namespace Plugins.CapTime
{
    [Export(typeof(IPlugin))]
    [Export(typeof(IStatus))]
    public class CapTimePlugin : IPlugin, IStatus
    {
        private IJob<CaptureArgs> worker;

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

                worker.Stop();

                return;
            }

            var intParams = state.Split(' ');

            int duration = Convert.ToInt32(intParams[0]);
            int interval = Convert.ToInt32(intParams[1]);

            string text = $"Screen capture has been scheduled to run {duration} sec for every {interval} sec.";

            var response = new Response(text, req.MessageId);

            await ResultHandler(response);

            ((IScheduled)worker).Start(duration, interval);
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

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            worker = new CaptureSchedule
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }

        public string GetStatus()
        {
            string text = "";

            string name = "Cap Time";
            bool active = worker.Active;

            text += $"*{name}*: {active.ToReadable()}\n";

            return text.TrimEnd('\n');
        }
    }
}
