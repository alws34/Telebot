using System;
using System.Threading.Tasks;
using Telebot.Infrastructure;
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

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            int brvalue = Convert.ToInt32(info.Groups[1].Value);

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully adjusted brightness to {brvalue}%."
            };

            await cbResult(result);

            systemApi.SetBrightness(brvalue);
        }
    }
}
