using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugins.Shutdown
{
    [Export(typeof(IPlugin))]
    public class ShutdownPlugin : IPlugin
    {
        public ShutdownPlugin()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";
            MinOSVersion = new Version(5, 1);
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int timeout = Convert.ToInt32(req.Groups[1].Value);

            string text = $"Workstation will shutdown in {timeout} seconds.";

            var result = new Response(text);

            await resp(result);

            ShutdownTimeout(timeout);
        }

        private void ShutdownTimeout(int timeout = 0)
        {
            var si = new ProcessStartInfo("shutdown", $"/s /t {timeout}")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(si);
        }
    }
}
