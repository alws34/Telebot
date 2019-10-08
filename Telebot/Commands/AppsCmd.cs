using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : CommandBase
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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var parameters = parameter as CommandParam;

            var act = parameters.Groups[1].Value;

            var res = actions[act].Invoke();

            var result = new CommandResult
            {
                Text = res,
                SendType = SendType.Text
            };

            await callback(result);
        }
    }
}
