using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCmd : BaseCommand
    {
        private readonly DesktopApi desktopApi;

        public CapAppCmd()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified application (by pid).";

            desktopApi = new DesktopApi();
        }

        public async override void Execute(CommandParam info, Func<CommandResult, Task> cbResult)
        {
            int pid = Convert.ToInt32(info.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var photo = desktopApi.CaptureWindow(hWnd);

            var result = new CommandResult
            {
                ResultType = ResultType.Photo,
                Raw = photo.ToStream()
            };

            await cbResult(result);
        }
    }
}
