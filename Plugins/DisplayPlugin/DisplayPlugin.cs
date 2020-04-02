using DisplayPlugin.Core;
using DisplayPlugin.Enums;
using System.Collections.Generic;
using BotSdk.Contracts;
using BotSdk.Models;

namespace Plugins.Display
{
    public class DisplayPlugin : IModule
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

            var result = new Response(
                $"Successfully turned {key} the monitor.",
                req.MessageId
            );

            await ResultHandler(result);

            DisplayState state = states[key];

            var api = new DisplayApi(state);

            api.Invoke();
        }
    }
}
