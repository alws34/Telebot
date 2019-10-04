using System;
using System.Diagnostics;
using Telebot.CoreApis;
using Telebot.Extensions;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CapAppCmd : CommandBase
    {
        private readonly DesktopApi desktopApi;

        public CapAppCmd()
        {
            Pattern = "/capapp (\\d+)";
            Description = "Get a screenshot of the specified application (by pid).";

            desktopApi = ApiLocator.Instance.GetService<DesktopApi>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var parameters = parameter as CommandParam;

            int pid = Convert.ToInt32(parameters.Groups[1].Value);

            var hWnd = Process.GetProcessById(pid).MainWindowHandle;

            var photo = desktopApi.CaptureWindow(hWnd);

            var cmdResult = new CommandResult
            {
                SendType = SendType.Photo,
                Stream = photo.ToStream()
            };

            callback(cmdResult);
        }
    }
}
