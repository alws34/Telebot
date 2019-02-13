using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.Monitors;

namespace Telebot.Commands
{
    public class TempTimeOffCmd : CommandBase
    {
        public TempTimeOffCmd()
        {
            Pattern = "/temptime off";
            Description = "Disables /temptime command.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully disabled scheduled temperature monitor."
            };

            ScheduledTempMonitor.Instance.Stop();

            callback(result);
        }
    }
}
