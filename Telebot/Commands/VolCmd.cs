using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCmd : CommandBase
    {
        private readonly MediaLogic mediaLogic;

        public VolCmd()
        {
            Pattern = "/vol (\\d+)";
            Description = "Adjust the workstation volume.";

            mediaLogic = new MediaLogic();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            double volume = Convert.ToDouble(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully adjusted the workstation volume."
            };

            callback(cmdResult);

            mediaLogic.SetVolume(volume);
        }
    }
}
