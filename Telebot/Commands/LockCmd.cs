using System;
using Telebot.Helpers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class LockCmd : CommandBase
    {
        public LockCmd()
        {
            Pattern = "/lock";
            Description = "Locks the workstation.";
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Locked down the workstation.",
                SendType = SendType.Text
            };

            callback(result);

            User32Helper.LockWorkStation();
        }
    }
}
