using Common.Models;
using Contracts;
using Contracts.Factories;
using Contracts.Jobs;
using CPUID.Base;
using SimpleInjector;
using System;
using System.ComponentModel.Composition;
using System.Text;
using TempTimePlugin.Jobs;
using TempTimePlugin.Models;

namespace Plugins.TempTime
{
    [Export(typeof(IPlugin))]
    public class TempTimeCommand : IPlugin
    {
        private IJob<TempArgs> worker;

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
                var resp1 = new Response($"Temperature monitor has turned {state}.");

                await respHandler(resp1);

                worker.Stop();

                return;
            }

            var args = state.Split(' ');

            int duration = Convert.ToInt32(args[0]);
            int interval = Convert.ToInt32(args[1]);

            string text = $"Temperature monitor has been scheduled to run {duration} sec for every {interval} sec.";

            var resp = new Response(text);

            await respHandler(resp);

            ((IScheduled)worker).Start(duration, interval);
        }

        StringBuilder text = new StringBuilder();

        private async void UpdateHandler(object sender, TempArgs e)
        {
            switch (e)
            {
                case null:
                    text.AppendLine("\nFrom *Telebot*");
                    var update = new Response(text.ToString());
                    await respHandler(update);
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

            await respHandler(result);
        }

        public override bool GetJobActive()
        {
            return worker.Active;
        }

        public override string GetJobName()
        {
            return "Temp Time";
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            var deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();

            worker = new TempSchedule(deviceFactory)
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }
    }
}
