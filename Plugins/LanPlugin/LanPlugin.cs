using BotSdk.Contracts;
using BotSdk.Extensions;
using BotSdk.Models;
using LanPlugin.Intranet;
using System;
using System.Collections.Generic;

namespace Plugins.Lan
{
    public class LanPlugin : IModule, IJobStatus
    {
        private IInetScanner lanScanner;
        private IINetMonitor lanMonitor;

        private Dictionary<string, Action> actions;

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

            actions[state].Invoke();
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

            lanScanner = new LanScanner
            {
                Discovered = HostHandler,
                Feedback = FeedbackHandler
            };

            lanMonitor = new LanMonitor
            {
                Connected = HostHandler,
                Disconnected = HostHandler,
                Feedback = FeedbackHandler
            };

            actions = new Dictionary<string, Action>
            {
                { "mon", lanMonitor.Listen },
                { "moff", lanMonitor.Disconnect },
                { "scan", lanScanner.Discover }
            };
        }

        public string GetStatus()
        {
            return $"*LAN Monitor*: {lanMonitor.Active.ToReadable()}";
        }
    }
}
