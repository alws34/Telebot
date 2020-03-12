using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class PowerCmd : ICommand
    {
        private readonly Dictionary<string, Action> actions;

        public PowerCmd()
        {
            Pattern = "/power (lock|logoff|sleep|reboot|shutdown)";
            Description = "Lock, logoff, sleep, reboot or shutdown the workstation.";

            var powerApi = new PowerApi();

            actions = new Dictionary<string, Action>()
            {
                { "lock", powerApi.LockWorkstation },
                { "logoff", powerApi.LogoffWorkstation },
                { "sleep", powerApi.SleepWorkstation },
                { "reboot", powerApi.RestartWorkstation },
                { "shutdown", powerApi.ShutdownWorkstation }
            };
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            string state = info.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully {state} the workstation."
            };

            await cbResult(result);

            actions[state].Invoke();
        }
    }
}
