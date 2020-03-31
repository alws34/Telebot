using Common.Models;
using Contracts;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Plugins.Shutdown
{
    [Export(typeof(IPlugin))]
    public class ShutdownPlugin : IPlugin
    {
        public ShutdownPlugin()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";
        }

        public override async void Execute(Request req)
        {
            int timeout = Convert.ToInt32(req.Groups[1].Value);

            string text = $"Workstation will shutdown in {timeout} seconds.";

            var result = new Response(text);

            await respHandler(result);

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
