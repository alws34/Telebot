using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class SleepCmd : CommandBase
    {
        private readonly PowerLogic powerLogic;

        public SleepCmd()
        {
            Pattern = "/sleep";
            Description = "Puts the workstation into sleep mode.";
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Workstation is entering sleep mode.",
                SendType = SendType.Text
            };

            callback(result);

            powerLogic.SleepWorkstation();
        }
    }
}
