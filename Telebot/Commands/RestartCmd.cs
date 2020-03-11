using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Models;

namespace Telebot.Commands
{
    public class RestartCmd : BaseCommand
    {
        public RestartCmd()
        {
            Pattern = "/restart";
            Description = "Restart Telebot.";
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = "Telebot is restarting..."
            };

            await cbResult(result);

            Application.Restart();
        }
    }
}
