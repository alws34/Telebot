using BotSdk.Contracts;
using BotSdk.Extensions;
using BotSdk.Jobs;
using BotSdk.Models;
using CPUID.Base;
using SimpleInjector;
using System.Linq;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Plugins.TempWarn
{
    public class DllMain : IModule, IJobStatus
    {
        private IJob<TempArgs> worker;

        public DllMain()
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

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

            var container = (Container)data.IoCProvider.GetService(typeof(Container));

            var cpus = container.GetAllInstances<IProcessor>();
            var gpus = container.GetAllInstances<IDisplay>();

            var devs = cpus.Concat<IDevice>(gpus);

            worker = new TempWarning(devs)
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
