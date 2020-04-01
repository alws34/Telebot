using Common.Contracts;
using Common.Extensions;
using Common.Models;
using Contracts.Jobs;
using CPUID.Base;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Plugins.TempWarn
{
    public class TempWarnPlugin : IPlugin, IModuleStatus
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

            await ResultHandler(response);

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

            var devices = data.IocContainer.GetAllInstances<IDevice>();

            worker = new TempWarning(devices)
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }

        public string GetStatus()
        {
            string text = "";

            string name = "Temp Monitor";
            bool active = worker.Active;

            text += $"*{name}*: {active.ToReadable()}\n";

            return text.TrimEnd('\n');
        }
    }
}
