using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class BrightCommand : ICommand
    {
        public BrightCommand()
        {
            Pattern = "/bright (\\d{1,3})";
            Description = "Adjust workstation's brightness.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int level = Convert.ToInt32(req.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted brightness to {level}%."
            };

            await resp(result);

            IApi api = new DimApi(level);

            api.Invoke();
        }
    }
}
