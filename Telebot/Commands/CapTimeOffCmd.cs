using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telebot.Models;
using Telebot.ScreenCaptures;

namespace Telebot.Commands
{
    public class CapTimeOffCmd : CommandBase
    {
        public CapTimeOffCmd()
        {
            Pattern = "/captime off";
            Description = "Disables /captime command.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully disabled scheduled screen capture."
            };

            ScheduledScreenCapture.Instance.Stop();

            callback(result);
        }
    }
}
