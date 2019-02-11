using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class RebootCmd : CommandBase
    {
        private readonly PowerLogic powerLogic;

        public RebootCmd()
        {
            Pattern = "/reboot";
            Description = "Reboots the workstation.";
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public override CommandResult Execute(object parameter)
        {
            var result = new CommandResult
            {
                Text = "Rebooting the workstation.",
                SendType = SendType.Text
            };

            powerLogic.RestartWorkstation();

            return result;
        }
    }
}
