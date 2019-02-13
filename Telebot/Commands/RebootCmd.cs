using System;
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

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Rebooting the workstation.",
                SendType = SendType.Text
            };

            callback(result);

            powerLogic.RestartWorkstation();
        }
    }
}
