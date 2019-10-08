using System;
using System.Threading.Tasks;
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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            int bright = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully adjusted brightness to {bright}%."
            };

            await callback(cmdResult);

            systemApi.SetBrightness(bright);
        }
    }
}
