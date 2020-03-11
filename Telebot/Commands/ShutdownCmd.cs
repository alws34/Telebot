using System;
using System.Threading.Tasks;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ShutdownCmd : BaseCommand
    {
        private readonly PowerApi powerApi;

        public ShutdownCmd()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";

            powerApi = new PowerApi();
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            int sec = Convert.ToInt32(info.Groups[1].Value);

            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = $"Successfully scheduled the workstation to shutdown in {sec} seconds."
            };

            await cbResult(result);

            powerApi.ShutdownWorkstation(sec);
        }
    }
}
