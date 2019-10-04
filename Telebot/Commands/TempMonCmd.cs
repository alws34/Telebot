using System;
using System.Collections.Generic;
using Telebot.Models;
using Telebot.Temperature;

namespace Telebot.Commands
{
    public class TempMonCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;

        public TempMonCmd()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";

            ITempMon tempMonWarning = Program.tempMonFactory.FindEntity(
                x => x.TempMonType == TempMonType.Warning
            );

            actions = new Dictionary<string, Action>()
            {
                { "on", tempMonWarning.Start },
                { "off", tempMonWarning.Stop }
            };
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            string state = parameters.Groups[1].Value;

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully turned {state} the temperature monitor."
            };

            callback(result);

            actions[state].Invoke();
        }
    }
}
