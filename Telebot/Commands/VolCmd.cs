using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCmd : ICommand
    {
        private readonly MediaApi mediaApi;

        public VolCmd()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";

            mediaApi = new MediaApi();
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            int vol = Convert.ToInt32(info.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted volume to {vol}%."
            };

            await cbResult(result);

            mediaApi.SetVolume(vol);
        }
    }
}
