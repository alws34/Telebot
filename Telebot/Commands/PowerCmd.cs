using System;
using System.Collections.Generic;
using Telebot.CoreApis;
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

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            string state = parameters.Groups[1].Value;

            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully {state} the workstation."
            };

            callback(result);

            actions[state].Invoke();
        }
    }
}
