using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Infrastructure.Apis;
using Telebot.Models;
using static Telebot.Native.user32;

namespace Telebot.Commands
{
    public class ScreenCommand : ICommand
    {
        private readonly Dictionary<string, DisplayState> states;

        public ScreenCommand()
        {
            Pattern = "/screen (on|off)";
            Description = "Turn off or on the monitor.";
            OSVersion = new Version(5, 0);

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

            IApi api = new DisplayApi(state);

            api.Invoke();
        }
    }
}
