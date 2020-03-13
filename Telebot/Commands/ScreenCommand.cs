using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenCommand : ICommand
    {
        private readonly Dictionary<string, Action> actions;

        public ScreenCommand()
        {
            Pattern = "/screen (on|off)";
            Description = "Turn off or on the monitor.";

            var display = new DisplayImpl();

            actions = new Dictionary<string, Action>()
            {
                { "on", display.SetDisplayOn },
                { "off", display.SetDisplayOff }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string state = req.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully turned {state} the monitor."
            };

            await resp(result);

            actions[state].Invoke();
        }
    }
}
