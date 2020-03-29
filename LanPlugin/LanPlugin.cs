using Contracts;
using LanPlugin.Intranet;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace LanPlugin
{
    [Export(typeof(IPlugin))]
    public class LanPlugin : IPlugin
    {
        private readonly IInetScanner scanner;
        private readonly IINetMonitor monitor;

        private readonly Dictionary<string, Action> methods;

        public LanPlugin()
        {
            Pattern = "/lan (mon|moff|scan)";
            Description = "Scan or listen for devices on the LAN.";
            MinOSVersion = new Version(5, 0);

            scanner = new LanScanner();
            monitor = new LanMonitor();

            methods = new Dictionary<string, Action>
            {
                { "mon", monitor.Listen },
                { "moff", monitor.Disconnect },
                { "scan", scanner.Discover }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            scanner.Discovered += async (s, e) =>
            {
                string text = "Discovered:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n\n";
                }

                var discovered = new Response(text);

                await resp(discovered);
            };

            monitor.Connected += async (s, e) =>
            {
                string text = "Connected:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n";
                }

                var connected = new Response(text);

                await resp(connected);
            };

            monitor.Disconnected += async (s, e) =>
            {
                string text = "Disconnected:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n";
                }

                var disconnected = new Response(text);

                await resp(disconnected);
            };

            string state = req.Groups[1].Value;

            var result = new Response($"Command {state} has been sent to network manager.");

            await resp(result);

            methods[state].Invoke();
        }

        public override bool GetJobActive()
        {
            return monitor.IsActive;
        }
    }
}
