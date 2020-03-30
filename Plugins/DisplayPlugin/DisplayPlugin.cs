using Common.Enums;
using Common.Models;
using Contracts;
using DisplayPlugin.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Plugins.Display
{
    [Export(typeof(IPlugin))]
    public class DisplayPlugin : IPlugin
    {
        private readonly Dictionary<string, DisplayState> states;

        public DisplayPlugin()
        {
            Pattern = "/screen (on|off)";
            Description = "Turn off or on the monitor.";
            MinOSVersion = new Version(5, 0);

            states = new Dictionary<string, DisplayState>()
            {
                { "on", DisplayState.DisplayStateOn },
                { "off", DisplayState.DisplayStateOff }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string key = req.Groups[1].Value;

            var result = new Response($"Successfully turned {key} the monitor.");

            await resp(result);

            DisplayState state = states[key];

            var api = new DisplayApi(state);

            api.Invoke();
        }
    }
}
