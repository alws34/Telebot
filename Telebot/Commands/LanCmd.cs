using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Models;

namespace Telebot.Commands
{
    public class LanCmd : ICommand
    {
        private readonly Dictionary<string, Action> methods;

        public LanCmd()
        {
            Pattern = "/lan (mon|moff|scan)";
            Description = "Scan or listen for connected/disconnected devices on the LAN.";

            methods = new Dictionary<string, Action>
            {
                { "mon", Program.LanMonitor.Listen },
                { "moff", Program.LanMonitor.Disconnect },
                { "scan", Program.LanScanner.Discover }
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
