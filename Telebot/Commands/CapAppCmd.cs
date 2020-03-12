using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCmd : ICommand
    {
        private readonly DesktopApi desktopApi;

        public CapAppCmd()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified application (by pid).";

            desktopApi = new DesktopApi();
        }

        public async override void Execute(Request info, Func<Response, Task> cbResult)
        {
            int pid = Convert.ToInt32(info.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var photo = desktopApi.CaptureWindow(hWnd);

            var result = new Response
            {
                ResultType = ResultType.Photo,
                Raw = photo.ToStream()
            };

            await cbResult(result);
        }
    }
}
