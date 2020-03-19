using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure.Apis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCommand : ICommand
    {
        public readonly Dictionary<string, AppsType> types;

        public AppsCommand()
        {
            Pattern = "/apps (fg|all)";
            Description = "List of active applications.";

            types = new Dictionary<string, AppsType>
            {
                { "fg", AppsType.Foreground },
                { "all", AppsType.Background }
            };
        }

        public override void Execute(Request req, Func<Response, Task> resp)
        {
            string key = req.Groups[1].Value;

            AppsType type = types[key];

            var api = new AppsApi(type);

            api.Invoke(async (s) =>
            {
                var result = new Response
                {
                    ResultType = ResultType.Text,
                    Text = s
                };

                await resp(result);
            });
        }
    }
}
