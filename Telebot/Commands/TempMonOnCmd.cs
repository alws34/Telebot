using System;
using System.Collections.Generic;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOnCmd : CommandBase
    {
        public TempMonOnCmd()
        {
            Pattern = "/tempmon on";
            Description = "Turn on temperature monitoring.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Temperature monitor is turned on.",
                SendType = SendType.Text
            };

            callback(result);

            PermanentTempMonitor.Instance.Start();
            Program.appSettings.TempMonEnabled = true;
        }
    }
}
