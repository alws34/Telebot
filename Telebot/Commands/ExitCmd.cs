using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ExitCmd : ICommand
    {
        public ExitCmd()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = "Closed Telebot."
            };

            await cbResult(result);

            Application.Exit();
        }
    }
}
