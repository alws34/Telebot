using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ScreenCmd : BaseCommand
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

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            string state = info.Groups[1].Value;

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully turned {state} the monitor."
            };

            await cbResult(result);

            actions[state].Invoke();
        }
    }
}
