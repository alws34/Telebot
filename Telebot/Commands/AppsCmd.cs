using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : ICommand
    {
        public readonly Dictionary<string, Func<string>> actions;

        public AppsCmd()
        {
            Pattern = "/apps (fg|all)";
            Description = "List of active applications.";

            var windowsApi = new WindowsApi();

            actions = new Dictionary<string, Func<string>>
            {
                { "fg", windowsApi.GetForegroundApps },
                { "all", windowsApi.GetBackgroundApps }
            };
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            string methName = req.Groups[1].Value;

            string methResult = actions[methName].Invoke();

            var result = new Response
            {
                Text = methResult,
                ResultType = ResultType.Text
            };

            await resp(result);
        }
    }
}
