using System;
using System.Collections.Generic;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;
        private readonly DisplayLogic displayLogic;

        public ScreenCmd()
        {
            Pattern = "/screen (on|off)";
            Description = "Turn off or on the monitor.";

            displayLogic = new DisplayLogic();

            actions = new Dictionary<string, Action>()
            {
                { "on", displayLogic.SetDisplayOn },
                { "off", displayLogic.SetDisplayOff }
            };
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            string state = parameters.Groups[1].Value;

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully turned {state} the monitor."
            };

            callback(result);

            actions[state].Invoke();
        }
    }
}
