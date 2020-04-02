using BotSdk.Contracts;
using BotSdk.Models;
using System;
using System.Diagnostics;

namespace Plugins.Shutdown
{
    public class DllMain : IModule
    {
        public DllMain()
        {
            Pattern = "/shutdown (\\d+)";
            Description = "Schedule the workstation to shutdown.";
        }

        public override async void Execute(Request req)
        {
            int timeout = Convert.ToInt32(req.Groups[1].Value);

            string text = $"Workstation will shutdown in {timeout} seconds.";

            var result = new Response(text, req.MessageId);

            await ResultHandler(result);

            ShutdownTimeout(timeout);
        }

        private void ShutdownTimeout(int timeout)
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
