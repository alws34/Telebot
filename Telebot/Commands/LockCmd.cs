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

        public override CommandResult Execute(object parameter)
        {
            var result = new CommandResult
            {
                Text = "Locked down the workstation.",
                SendType = SendType.Text
            };

            User32Helper.LockWorkStation();

            return result;
        }
    }
}
