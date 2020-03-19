using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class VolCommand : ICommand
    {
        public VolCommand()
        {
            Pattern = "/vol (\\d{1,3})";
            Description = "Adjust workstation's volume.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int vol = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted volume to {vol}%."
            };

            await resp(result);

            IApi api = new VolApi(vol);

            api.Invoke();
        }
    }
}
