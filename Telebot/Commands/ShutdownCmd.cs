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
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";

            powerLogic = new PowerLogic();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            var time = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully scheduled the workstation to shutdown in {time} seconds."
            };

            callback(cmdResult);

            powerLogic.ShutdownWorkstation(time);
        }
    }
}
