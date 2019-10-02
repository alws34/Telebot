using System;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : CommandBase
    {
        private readonly PowerApi powerLogic;

        public ShutdownCmd()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";

            powerLogic = new PowerApi();
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
