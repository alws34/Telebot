using Common.Contracts;
using Common.Extensions;
using Common.Models;
using LanPlugin.Intranet;

namespace Plugins.Lan
{
    public class LanPlugin : IModule, IJobStatus
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

            var response = new Response(
                $"Lan triggered to {state}.",
                req.MessageId
            );

            await ResultHandler(response);

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

            var result = new Response(text);

            await ResultHandler(result);
        }

        private async void FeedbackHandler(object sender, Feedback e)
        {
            var result = new Response(e.Text);

            await ResultHandler(result);
        }

        public override void Initialize(ModuleData data)
        {
            base.Initialize(data);

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

        public string GetStatus()
        {
            string text = "";

            string name = "LAN Monitor";
            bool active = monitor.IsActive;

            text += $"*{name}*: {active.ToReadable()}\n";

            return text.TrimEnd('\n');
        }
    }
}
