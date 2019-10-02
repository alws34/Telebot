using System;
using System.Drawing;
using Telebot.Extensions;
using Telebot.CoreApis;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : CommandBase
    {
        private readonly DesktopApi desktopApi;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";

            desktopApi = ApiLocator.Instance.GetService<DesktopApi>();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var photos = desktopApi.CaptureDesktop();

            foreach (Bitmap photo in photos)
            {
                var result = new CommandResult
                {
                    Stream = photo.ToStream(),
                    SendType = SendType.Photo
                };

                callback(result);
            }
        }
    }
}
