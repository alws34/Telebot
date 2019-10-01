using System;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCmd : CommandBase
    {
        private readonly MediaLogic mediaLogic;

        public VolCmd()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";

            mediaLogic = new MediaLogic();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int vol = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully adjusted volume to {vol}%."
            };

            callback(cmdResult);

            mediaLogic.SetVolume(vol);
        }
    }
}
