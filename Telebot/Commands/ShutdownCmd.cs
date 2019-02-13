using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : CommandBase
    {
        private readonly PowerLogic powerLogic;

        public ShutdownCmd()
        {
            Pattern = "/shutdown";
            Description = "Shuts down the workstation.";
            powerLogic = Program.container.GetInstance<PowerLogic>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var result = new CommandResult
            {
                Text = "Shutting down the workstation.",
                SendType = SendType.Text
            };

            callback(result);

            powerLogic.ShutdownWorkstation();
        }
    }
}
