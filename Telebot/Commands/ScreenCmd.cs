using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;

        public ScreenCmd()
        {
            Pattern = "/screen (on|off)";
            Description = "Turn off or on the monitor.";

            var displayApi = ApiLocator.Instance.GetService<DisplayApi>();

            actions = new Dictionary<string, Action>()
            {
                { "on", displayApi.SetDisplayOn },
                { "off", displayApi.SetDisplayOff }
            };
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            string state = parameters.Groups[1].Value;

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully turned {state} the monitor."
            };

            await callback(result);

            actions[state].Invoke();
        }
    }
}
