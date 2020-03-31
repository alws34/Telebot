using Common.Enums;
using Common.Models;
using Contracts;
using DisplayPlugin.Core;
using System.Collections.Generic;
using System.ComponentModel.Composition;

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

            states = new Dictionary<string, DisplayState>()
            {
                { "on", DisplayState.DisplayStateOn },
                { "off", DisplayState.DisplayStateOff }
            };
        }

        public override async void Execute(Request req)
        {
            string key = req.Groups[1].Value;

            var result = new Response($"Successfully turned {key} the monitor.");

            await resultHandler(result);

            DisplayState state = states[key];

            var api = new DisplayApi(state);

            api.Invoke();
        }
    }
}
