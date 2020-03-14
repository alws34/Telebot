using FluentScheduler;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class ExitCommand : ICommand
    {
        public ExitCommand()
        {
            Pattern = "/exit";
            Description = "Shutdown Telebot.";
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = "Closing Telebot..."
            };

            await resp(result);

            JobManager.AddJob(() =>
            {
                Application.Exit();
            }, (s) => s.ToRunOnceIn(2).Seconds()
            );
        }
    }
}
