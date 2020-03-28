using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Intranet;
using Telebot.Models;

namespace Telebot.Commands
{
    public class LanCommand : ICommand
    {
        private readonly Dictionary<string, Action> methods;

        public LanCommand()
        {
            Pattern = "/lan (mon|moff|scan)";
            Description = "Scan or listen for devices on the LAN.";
            OSVersion = new Version(5, 0);

            var scanner = Program.InetFactory.FindEntity(
                x => x.Jobtype == JobType.Scanner
            ) as IInetScanner;

            var monitor = Program.InetFactory.FindEntity(
                x => x.Jobtype == JobType.Monitor
            ) as IINetMonitor;

            methods = new Dictionary<string, Action>
            {
                { "mon", monitor.Listen },
                { "moff", monitor.Disconnect },
                { "scan", scanner.Discover }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string state = req.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Command {state} has been sent to network manager."
            };

            await resp(result);

            methods[state].Invoke();
        }
    }
}
