using System;
using System.Threading.Tasks;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCmd : CommandBase
    {
        private readonly MediaApi mediaApi;

        public VolCmd()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";

            mediaApi = ApiLocator.Instance.GetService<MediaApi>();
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            int vol = Convert.ToInt32(parameters.Groups[1].Value);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = $"Successfully adjusted volume to {vol}%."
            };

            await callback(cmdResult);

            mediaApi.SetVolume(vol);
        }
    }
}
