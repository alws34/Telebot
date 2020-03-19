using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class PowerCommand : ICommand
    {
        private readonly Dictionary<string, PowerType> types;

        public PowerCommand()
        {
            Pattern = "/power (lock|logoff|sleep|reboot|shutdown)";
            Description = "Lock, logoff, sleep, reboot or shutdown the workstation.";

            types = new Dictionary<string, PowerType>()
            {
                { "lock", PowerType.Lock },
                { "logoff", PowerType.Logoff },
                { "sleep", PowerType.Sleep },
                { "reboot", PowerType.Restart },
                { "shutdown", PowerType.Shutdown }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string key = req.Groups[1].Value;

            var result = new Response
            {
                ResultType = ResultType.Text,
                Text = $"Successfully {key} the workstation."
            };

            await resp(result);

            PowerType type = types[key];

            IApi api = new PowerApi(type);

            ApiInvoker.Instance.Invoke(api);
        }
    }
}
