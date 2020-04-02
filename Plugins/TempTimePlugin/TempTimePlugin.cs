using Common.Contracts;
using Common.Extensions;
using Common.Models;
using Contracts.Jobs;
using CPUID.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempTimePlugin.Jobs;
using TempTimePlugin.Models;

namespace Plugins.TempTime
{
    public class TempTimeCommand : IModule, IJobStatus
    {
        private IJob<TempArgs> job;

        public TempTimeCommand()
        {
            Pattern = "/temptime (off|(\\d+) (\\d+))";
            Description = "Schedules temperature monitor.";
        }

        public override async void Execute(Request req)
        {
            string state = req.Groups[1].Value;

            if (state.Equals("off"))
            {
                var resp1 = new Response(
                    $"Temperature monitor has turned {state}.",
                    req.MessageId
                );

                await ResultHandler(resp1);
                job.Stop();
                return;
            }

            var args = state.Split(' ');

            int duration = Convert.ToInt32(args[0]);
            int interval = Convert.ToInt32(args[1]);

            string text = $"Temperature monitor has been scheduled to run {duration} sec for every {interval} sec.";

            var resp = new Response(text, req.MessageId);

            await ResultHandler(resp);

            ((IScheduled)job).Start(duration, interval);
        }

        StringBuilder text = new StringBuilder();

        private async void UpdateHandler(object sender, TempArgs e)
        {
            switch (e)
            {
                case null:
                    text.AppendLine("\nFrom *Telebot*");
                    var update = new Response(text.ToString());
                    await ResultHandler(update);
                    text.Clear();
                    break;
                default:
                    text.AppendLine($"*{e.DeviceName}*: {e.Temperature}°C");
                    break;
            }
        }

        private async void FeedbackHandler(object sender, Feedback e)
        {
            var result = new Response(e.Text);
            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            var cpus = data.IocContainer.GetAllInstances<IProcessor>();
            var gpus = data.IocContainer.GetAllInstances<IDisplay>();

            var devs = cpus.Concat<IDevice>(gpus);

            job = new TempSchedule(devs)
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }

        public string GetStatus()
        {
            string text = "";

            string name = "Temp Time";
            bool active = job.Active;

            text += $"*{name}*: {active.ToReadable()}\n";

            return text.TrimEnd('\n');
        }
    }
}
