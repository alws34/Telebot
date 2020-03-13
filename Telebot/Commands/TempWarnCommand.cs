﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class TempWarnCommand : ICommand
    {
        private readonly Dictionary<string, Action> actions;

        public TempWarnCommand()
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

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string state = req.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully turned {state} the temperature monitor."
            };

            await resp(result);

            actions[state].Invoke();
        }
    }
}