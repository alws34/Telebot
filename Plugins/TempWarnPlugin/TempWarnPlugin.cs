using Common.Models;
using Contracts;
using Contracts.Factories;
using Contracts.Jobs;
using CPUID.Base;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Plugins.TempWarn
{
    [Export(typeof(IPlugin))]
    public class TempWarnPlugin : IPlugin
    {
        private Dictionary<string, Action> actions;
        private IJob<TempArgs> worker;

        public TempWarnPlugin()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";
            MinOsVersion = new Version(5, 1);
        }

        public override async void Execute(Request req, Func<Response, Task> resp)
        {
            worker.Update = async (s, e) =>
            {
                string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

                var update = new Response(text, false);

                await resp(update);
            };

            worker.Feedback = async (s, e) =>
            {
                var result = new Response(e.Text);

                await resp(result);
            };

            string state = req.Groups[1].Value;

            var response = new Response($"Successfully sent \"{state}\" to the temperature monitor.");

            await resp(response);

            actions[state].Invoke();
        }

        public override bool GetJobActive()
        {
            return worker.Active;
        }

        public override string GetJobName()
        {
            return "Temp Monitor";
        }

        public override void Initialize(Container iocContainer)
        {
            var deviceFactory = iocContainer.GetInstance<IFactory<IDevice>>();

            worker = new TempWarning(deviceFactory);

            actions = new Dictionary<string, Action>()
            {
                { "on", worker.Start },
                { "off", worker.Stop }
            };
        }
    }
}
