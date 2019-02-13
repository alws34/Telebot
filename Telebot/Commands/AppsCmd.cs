using System;
using Telebot.BusinessLogic;
using Telebot.Models;

namespace Telebot.Commands
{
    public class AppsCmd : CommandBase
    {
        private readonly WindowsLogic windowsLogic;

        public AppsCmd()
        {
            Pattern = "/apps";
            Description = "List of active applications.";
            windowsLogic = Program.container.GetInstance<WindowsLogic>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            string activeApps = windowsLogic.GetActiveApplications();

            var result = new CommandResult
            {
                Text = activeApps,
                SendType = SendType.Text
            };

            callback(result);
        }
    }
}
