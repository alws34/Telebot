using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
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
            MinOsVersion = new Version(5, 0);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string text = req.Groups[1].Value;

            var result = new Response("Successfully displayed alert.");

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
