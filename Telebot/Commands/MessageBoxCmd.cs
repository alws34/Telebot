using System;
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

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            var msg = parameters.Groups[1].Value;

            var cmdResult = new CommandResult
            {
                SendType = SendType.Text,
                Text = "Successfully initiated a message box."
            };

            callback(cmdResult);

            MessageBox.Show(msg);
        }
    }
}
