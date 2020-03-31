using Common.Enums;
using Common.Models;
using Contracts;
using PowerPlugin.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

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
            MinOsVersion = new Version(5, 1);

            types = new Dictionary<string, PowerType>()
            {
                { "lock", PowerType.Lock },
                { "logoff", PowerType.Logoff },
                { "sleep", PowerType.Sleep },
                { "reboot", PowerType.Restart },
                { "shutdown", PowerType.Shutdown }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string key = req.Groups[1].Value;

            var result = new Response($"Workstation is going to {key}..");

            await resp(result);

            PowerType type = types[key];

            var api = new PowerApi(type);

            api.Invoke();
        }
    }
}
