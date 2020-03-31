using Common.Models;
using Contracts;
using Contracts.Factories;
using Contracts.Jobs;
using CPUID.Base;
using SimpleInjector;
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

            var response = new Response($"Temperature monitor has turned {state}.");

            await respHandler(response);

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

            var update = new Response(text, false);

            await respHandler(update);
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
            return "Temp Monitor";
        }

        public override void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            var deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();

            worker = new TempWarning(deviceFactory)
            {
                Update = UpdateHandler,
                Feedback = FeedbackHandler
            };
        }
    }
}
