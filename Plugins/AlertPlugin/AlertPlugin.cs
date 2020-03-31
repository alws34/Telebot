using Common.Models;
using Contracts;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace Plugins.Alert
{
    [Export(typeof(IPlugin))]
    public class AlertPlugin : IPlugin
    {
        public AlertPlugin()
        {
            Pattern = "/alert \"(.+?)\"";
            Description = "Display an alert with the specified text.";

        }

        public override async void Execute(Request req)
        {
            string text = req.Groups[1].Value;

            var result = new Response("Alert has been displayed.");

            await resultHandler(result);

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
