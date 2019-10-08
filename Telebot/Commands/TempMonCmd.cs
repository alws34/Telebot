using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class TempMonCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;

        public TempMonCmd()
        {
            Pattern = "/tempmon (on|off)";
            Description = "Turn on or off the temperature monitor.";

            var tempMonWarning = Program.tempMonFactory.FindEntity(
                x => x.JobType == JobType.Fixed
            );

            actions = new Dictionary<string, Action>()
            {
                { "on", tempMonWarning.Start },
                { "off", tempMonWarning.Stop }
            };
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            string state = parameters.Groups[1].Value;

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully turned {state} the temperature monitor."
            };

            await callback(result);

            actions[state].Invoke();
        }
    }
}
