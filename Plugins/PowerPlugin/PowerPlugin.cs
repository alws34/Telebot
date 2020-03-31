using Common.Enums;
using Common.Models;
using Contracts;
using PowerPlugin.Core;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Plugins.Power
{
    [Export(typeof(IPlugin))]
    public class PowerPlugin : IPlugin
    {
        private readonly Dictionary<string, PowerType> types;

        public PowerPlugin()
        {
            Pattern = "/power (lock|logoff|sleep|reboot|shutdown)";
            Description = "Lock, logoff, sleep, reboot or shutdown the workstation.";

            types = new Dictionary<string, PowerType>()
            {
                { "lock", PowerType.Lock },
                { "logoff", PowerType.Logoff },
                { "sleep", PowerType.Sleep },
                { "reboot", PowerType.Restart },
                { "shutdown", PowerType.Shutdown }
            };
        }

        public override async void Execute(Request req)
        {
            string key = req.Groups[1].Value;

            var result = new Response(
                $"Workstation is going to {key}..", 
                req.MessageId
            );

            await resultHandler(result);

            PowerType type = types[key];

            var api = new PowerApi(type);

            api.Invoke();
        }
    }
}
