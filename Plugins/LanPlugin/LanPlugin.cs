using Contracts;
using LanPlugin.Intranet;
using Common.Models;
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
            scanner.Discovered = async (s, e) =>
            {
                string text = "Discovered:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n\n";
                }

                var result = new Response(text, false);

                await resp(result);
            };

            scanner.Feedback = async (s, e) =>
            {
                var result = new Response(e.Text);

                await resp(result);
            };

            monitor.Connected = async (s, e) =>
            {
                string text = "Connected:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n";
                }

                var result = new Response(text, false);

                await resp(result);
            };

            monitor.Disconnected = async (s, e) =>
            {
                string text = "Disconnected:\n\n";

                foreach (Host host in e.Hosts)
                {
                    text += host.ToString();
                    text += "\n";
                }

                var result = new Response(text, false);

                await resp(result);
            };

            monitor.Feedback = async (s, e) =>
            {
                var result = new Response(e.Text);

                await resp(result);
            };

            string state = req.Groups[1].Value;

            var response = new Response($"Lan triggered to {state}.");

            await resp(response);

            methods[state].Invoke();
        }

        public override bool GetJobActive()
        {
            return monitor.IsActive;
        }

        public override string GetJobName()
        {
            return "LAN Monitor";
        }
    }
}
