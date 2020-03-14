using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telebot.Common;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCommand : ICommand
    {
        private readonly ScreenImpl screen;

        public CapAppCommand()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified app (by pid).";

            screen = new ScreenImpl();
        }

        public async override void Execute(Request req, Func<Response, Task> resp)
        {
            int pid = Convert.ToInt32(req.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var wnd = screen.CaptureWindow(hWnd);

            var result = new Response
            {
                ResultType = ResultType.Photo,
                Raw = wnd.ToStream()
            };

            await resp(result);
        }
    }
}
