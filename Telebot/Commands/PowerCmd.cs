using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class PowerCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;

        public PowerCmd()
        {
            Pattern = "/(lock|logoff|sleep|reboot|shutdown)";
            Description = "Lock, logoff, sleep, reboot or shutdown the workstation.";

            var powerApi = ApiLocator.Instance.GetService<PowerApi>();

            actions = new Dictionary<string, Action>()
            {
                { "lock", powerApi.LockWorkstation },
                { "logoff", powerApi.LogoffWorkstation },
                { "sleep", powerApi.SleepWorkstation },
                { "reboot", powerApi.RestartWorkstation },
                { "shutdown", powerApi.ShutdownWorkstation }
            };
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            string state = info.Groups[1].Value;

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully {state} the workstation."
            };

            await cbResult(result);

            actions[state].Invoke();
        }
    }
}
