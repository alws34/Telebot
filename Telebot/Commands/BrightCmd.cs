using System;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class BrightCmd : ICommand
    {
        private readonly SystemApi systemApi;

        public BrightCmd()
        {
            Pattern = "/bright (\\d{1,3})";
            Description = "Adjust workstation's brightness.";

            systemApi = new SystemApi();
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            int brvalue = Convert.ToInt32(info.Groups[1].Value);

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted brightness to {brvalue}%."
            };

            await cbResult(result);

            systemApi.SetBrightness(brvalue);
        }
    }
}
