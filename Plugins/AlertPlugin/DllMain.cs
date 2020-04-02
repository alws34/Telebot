using BotSdk.Contracts;
using BotSdk.Models;
using System.Windows.Forms;

namespace Plugins.Alert
{
    public class DllMain : IModule
    {
        public DllMain()
        {
            Pattern = "/alert \"(.+?)\"";
            Description = "Display an alert.";
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
