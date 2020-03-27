using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AlertCommand : ICommand
    {
        public AlertCommand()
        {
            Pattern = "/message \"(.+?)\"";
            Description = "Display a message box with the specified text.";
            OSVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string msg = req.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = "Successfully initiated a message box."
            };

            await resp(result);

            MessageBox.Show
            (
                msg,
                "Telebot",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly
            );
        }
    }
}
