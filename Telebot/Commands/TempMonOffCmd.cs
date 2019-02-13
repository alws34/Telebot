using System;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempMonOffCmd : CommandBase
    {
        public TempMonOffCmd()
        {
            Pattern = "/tempmon off";
            Description = "Turn off temperature monitoring.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Temperature monitor is turned off.",
                SendType = SendType.Text
            };

            callback(result);

            PermanentTempMonitor.Instance.Stop();
            Program.appSettings.TempMonEnabled = false;
        }
    }
}
