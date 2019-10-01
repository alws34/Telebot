using System;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class BrightCmd : CommandBase
    {
        private readonly SystemLogic mediaLogic;

        public BrightCmd()
        {
            Pattern = "/bright (\\d{1,3})";
            Description = "Adjust workstation's brightness.";

            mediaLogic = new SystemLogic();
        }
        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int bright = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully adjusted brightness to {bright}%."
            };

            callback(cmdResult);

            mediaLogic.SetBrightness(bright);
        }
    }
}
