﻿using System;
using System.Collections.Generic;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class PowerCmd : CommandBase
    {
        private readonly Dictionary<string, Action> actions;

        private readonly PowerApi powerLogic;

        public PowerCmd()
        {
            Pattern = "/(lock|logoff|sleep|reboot|shutdown)";
            Description = "Lock, logoff, sleep, reboot or shutdown the workstation.";

            powerLogic = new PowerApi();

            actions = new Dictionary<string, Action>()
            {
                { "lock", powerLogic.LockWorkstation },
                { "logoff", powerLogic.LogoffWorkstation },
                { "sleep", powerLogic.SleepWorkstation },
                { "reboot", powerLogic.RestartWorkstation },
                { "shutdown", powerLogic.ShutdownWorkstation }
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
