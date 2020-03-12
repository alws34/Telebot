using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class LanMonCmd : ICommand
    {
        private readonly Dictionary<string, Action> methods;

        public LanMonCmd()
        {
            Pattern = "/lan (mon|moff|scan)";
            Description = "Scan or listen for connected/disconnected devices on the LAN.";

            methods = new Dictionary<string, Action>
            {
                { "mon", Program.NetMonitor.Listen },
                { "moff", Program.NetMonitor.Disconnect },
                { "scan", Program.NetMonitor.Discover }
            };
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            string state = info.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Command {state} has been sent to network listener."
            };

            await cbResult(result);

            methods[state].Invoke();
        }
    }
}
