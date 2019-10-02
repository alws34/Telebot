using System;
using System.Diagnostics;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCmd : CommandBase
    {
        private readonly DesktopApi captureLogic;

        public CapAppCmd()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified application (by pid).";

            captureLogic = new DesktopApi();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int pid = Convert.ToInt32(parameters.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var photo = captureLogic.CaptureWindow(hWnd);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Photo,
                Stream = photo.ToStream()
            };

            callback(cmdResult);
        }
    }
}
