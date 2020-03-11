using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : BaseCommand
    {
        public readonly Dictionary<string, Func<string>> actions;

        public AppsCmd()
        {
            Pattern = "/apps (fg|all)";
            Description = "List of active applications.";

            var windowsApi = ApiLocator.Instance.GetService<WindowsApi>();

            actions = new Dictionary<string, Func<string>>
            {
                { "fg", windowsApi.GetForegroundApps },
                { "all", windowsApi.GetBackgroundApps }
            };
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            string methName = info.Groups[1].Value;

            string methResult = actions[methName].Invoke();

            var result = new CommandResult
            {
                Text = methResult,
                ResultType = ResultType.Text
            };

            await cbResult(result);
        }
    }
}
