using System;
using System.Threading.Tasks;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : CommandBase
    {
        private readonly PowerApi powerApi;

        public ShutdownCmd()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";

            powerApi = ApiLocator.Instance.GetService<PowerApi>();
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            var time = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully scheduled the workstation to shutdown in {time} seconds."
            };

            await callback(cmdResult);

            powerApi.ShutdownWorkstation(time);
        }
    }
}
