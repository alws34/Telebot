using System;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            string text = req.Groups[1].Value;

            var result = new Response("Successfully displayed a message box.");

            await resp(result);

            MessageBox.Show(
                text,
                "Telebot",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly
            );
        }
    }
}
