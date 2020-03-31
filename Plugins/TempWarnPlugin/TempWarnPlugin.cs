using Common.Models;
using Contracts;
using Contracts.Factories;
using Contracts.Jobs;
using CPUID.Base;
using System.ComponentModel.Composition;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Plugins.TempWarn
{
    [Export(typeof(IPlugin))]
    public class TempWarnPlugin : IPlugin
    {
        private IJob<TempArgs> worker;

        public TempWarnPlugin()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";
        }

        public override async void Execute(Request req)
        {
            string state = req.Groups[1].Value;

            var response = new Response(
                $"Temperature monitor has turned {state}.",
                req.MessageId
            );

            await resultHandler(response);

            switch (state)
            {
                case "on":
                    worker.Start();
                    break;
                case "off":
                    worker.Stop();
                    break;
            }
        }

        private async void UpdateHandler(object sender, TempArgs e)
        {
            string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

            var update = new Response(text);

            await resultHandler(update);
        }

        private async void FeedbackHandler(object sender, Feedback e)
        {
            var result = new Response(e.Text);

            await resultHandler(result);
        }

        public override bool GetJobActive()
        {
            return worker.Active;
        }

        public override string GetJobName()
        {
            return "Temp Monitor";
        }

        public override void Initialize(PluginData data)
        {
            base.Initialize(data);

            var deviceFactory = data.iocContainer.GetInstance<IFactory<IDevice>>();

            worker = new TempWarning(deviceFactory)
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }
    }
}
