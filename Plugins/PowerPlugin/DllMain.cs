﻿using BotSdk.Contracts;
using BotSdk.Models;
using PowerPlugin.Core;
using PowerPlugin.Enums;
using System.Collections.Generic;

namespace Plugins.Power
{
    public class DllMain : IModule
    {
        private readonly Dictionary<string, PowerType> types;

        public DllMain()
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

            await ResultHandler(result);

            PowerType type = types[key];

            var api = new PowerApi(type);

            api.Invoke();
        }
    }
}
