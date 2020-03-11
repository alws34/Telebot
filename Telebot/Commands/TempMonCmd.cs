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

            var tempMonWarning = Program.TempFactory.FindEntity(
                x => x.JobType == JobType.Fixed
            );

            actions = new Dictionary<string, Action>()
            {
                { "on", tempMonWarning.Start },
                { "off", tempMonWarning.Stop }
            };
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            string state = info.Groups[1].Value;

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully turned {state} the temperature monitor."
            };

            await cbResult(result);

            actions[state].Invoke();
        }
    }
}
