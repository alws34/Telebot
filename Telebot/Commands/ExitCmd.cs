using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ExitCmd : BaseCommand
    {
        public ExitCmd()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            var result = new CommandResult
            {
                ResultType = ResultType.Text,
                Text = "Closed Telebot."
            };

            await cbResult(result);

            Application.Exit();
        }
    }
}
