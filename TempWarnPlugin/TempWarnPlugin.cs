using Contracts;
using CPUID;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TempWarnPlugin.Jobs;
using TempWarnPlugin.Models;

namespace Telebot.Commands
{
    public class TempWarnPlugin : IPlugin
    {
        private readonly Dictionary<string, Action> actions;
        private readonly IJob<TempArgs> _job;

        public TempWarnPlugin()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";
            MinOSVersion = new Version(5, 1);

            var devicesToMonitor = CpuIdWrapper64.DeviceFactory.FindAll(x =>
               (x.DeviceClass == CPUID.Sdk.CpuIdSdk64.CLASS_DEVICE_PROCESSOR) ||
               (x.DeviceClass == CPUID.Sdk.CpuIdSdk64.CLASS_DEVICE_DISPLAY_ADAPTER)
            );

            _job = new TempWarning(devicesToMonitor);

            actions = new Dictionary<string, Action>()
            {
                { "on", _job.Start },
                { "off", _job.Stop }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            _job.Update += async (s, e) =>
            {
                string text = $"*[WARNING] {e.DeviceName}*: {e.Temperature}°C\nFrom *Telebot*";

                var update = new Response(text);

                await resp(update);
            };

            string state = req.Groups[1].Value;

            var result = new Response($"Successfully sent \"{state}\" to the temperature monitor.");

            await resp(result);

            actions[state].Invoke();
        }

        public override bool GetJobActive()
        {
            return _job.Active;
        }
    }
}
