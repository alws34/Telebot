using System;
using System.Collections.Generic;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : CommandBase
    {
        private readonly WindowsLogic windowsLogic;
        public readonly Dictionary<string, Func<string>> actions;

        public AppsCmd()
        {
            Pattern = "/apps (fg|all)";
            Description = "List of active applications.";

            windowsLogic = new WindowsLogic();

            actions = new Dictionary<string, Func<string>>
            {
                { "fg", windowsLogic.GetForegroundProcesses },
                { "all", windowsLogic.GetBackgroundProcesses }
            };
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            var act = parameters.Groups[1].Value;

            var res = actions[act].Invoke();

            var result = new CommandResult
            {
                Text = res,
                SendType = SendType.Text
            };

            callback(result);
        }
    }
}
