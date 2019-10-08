using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telebot.Models;

namespace Telebot.Commands
{
    public class MessageBoxCmd : CommandBase
    {
        public MessageBoxCmd()
        {
            Pattern = "/message \"(.+?)\"";
            Description = "Shows a message box with the specified text.";
        }

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            var msg = parameters.Groups[1].Value;

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully initiated a message box."
            };

            await callback(cmdResult);

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
