using System;
using System.Threading.Tasks;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCmd : BaseCommand
    {
        private readonly MediaApi mediaApi;

        public VolCmd()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";

            mediaApi = ApiLocator.Instance.GetService<MediaApi>();
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            int vol = Convert.ToInt32(info.Groups[1].Value);

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted volume to {vol}%."
            };

            await cbResult(result);

            mediaApi.SetVolume(vol);
        }
    }
}
