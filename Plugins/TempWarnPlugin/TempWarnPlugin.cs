using Contracts;
using Contracts.Jobs;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Telebot.Commands
{
    [Export(typeof(IPlugin))]
    public class TempWarnPlugin : IPlugin
    {
        private readonly Dictionary<string, Action> actions;
        private readonly IJob<TempArgs> _job;

        public TempWarnPlugin()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";
            MinOSVersion = new Version(5, 1);

            _job = new TempWarning();

            actions = new Dictionary<string, Action>()
            {
                { "on", _job.Start },
                { "off", _job.Stop }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            _job.Update = async (s, e) =>
            {
                string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

                var update = new Response(text, false);

                await resp(update);
            };

            _job.Feedback = async (s, e) =>
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
            return _job.Active;
        }

        public override string GetJobName()
        {
            return "Temp Monitor";
        }
    }
}
