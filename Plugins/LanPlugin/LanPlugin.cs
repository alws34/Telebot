using Common.Models;
using Contracts;
using LanPlugin.Intranet;
using System.ComponentModel.Composition;

namespace Plugins.Lan
{
    [Export(typeof(IPlugin))]
    public class LanPlugin : IPlugin
    {
        private IInetScanner scanner;
        private IINetMonitor monitor;

        public LanPlugin()
        {
            Pattern = "/lan (mon|moff|scan)";
            Description = "Scan or listen for devices on the LAN.";
        }

        public override async void Execute(Request req)
        {
            string state = req.Groups[1].Value;

            var response = new Response($"Lan triggered to {state}.");

            await respHandler(response);

            switch (state)
            {
                case "mon":
                    monitor.Listen();
                    break;
                case "moff":
                    monitor.Disconnect();
                    break;
                case "scan":
                    scanner.Discover();
                    break;
            }
        }

        private async void HostHandler(object sender, HostsArg e)
        {
            string text = $"{e.State}:\n\n";

            foreach (Host host in e.Hosts)
            {
                text += host.ToString();
                text += "\n\n";
            }

            var result = new Response(text, false);

            await respHandler(result);
        }

        private async void FeedbackHandler(object sender, Feedback e)
        {
            var result = new Response(e.Text);

            await respHandler(result);
        }

        public override bool GetJobActive()
        {
            return monitor.IsActive;
        }

        public override string GetJobName()
        {
            return "LAN Monitor";
        }

        public override void Initialize(ResponseHandler respHandler)
        {
            base.Initialize(respHandler);

            scanner = new LanScanner
            {
                Discovered = HostHandler,
                Feedback = FeedbackHandler
            };

            monitor = new LanMonitor
            {
                Connected = HostHandler,
                Disconnected = HostHandler,
                Feedback = FeedbackHandler
            };
        }
    }
}
