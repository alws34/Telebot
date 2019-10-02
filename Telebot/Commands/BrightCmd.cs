using System;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class BrightCmd : CommandBase
    {
        private readonly SystemApi systemApi;

        public BrightCmd()
        {
            Pattern = "/bright (\\d{1,3})";
            Description = "Adjust workstation's brightness.";

            systemApi = ApiLocator.Instance.GetService<SystemApi>();
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

            systemApi.SetBrightness(bright);
        }
    }
}
