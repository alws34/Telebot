using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class MessageBoxCmd : ICommand
    {
        public MessageBoxCmd()
        {
            Pattern = "/message \"(.+?)\"";
            Description = "Shows a message box with the specified text.";
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            string msg = info.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = "Successfully initiated a message box."
            };

            await cbResult(result);

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
