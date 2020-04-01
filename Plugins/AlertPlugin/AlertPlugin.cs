using Common.Models;
using System.Windows.Forms;
using Common.Contracts;

namespace Plugins.Alert
{

    public class AlertPlugin : IModule
    {
        public AlertPlugin()
        {
            Pattern = "/alert \"(.+?)\"";
            Description = "Display an alert with the specified text.";

        }

        public override async void Execute(Request req)
        {
            string text = req.Groups[1].Value;

            var result = new Response("Alert has been displayed.", req.MessageId);

            await ResultHandler(result);

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
