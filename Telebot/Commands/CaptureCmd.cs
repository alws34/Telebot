using System;
using System.Drawing;
using Telebot.Extensions;
using Telebot.Infrastructure;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : CommandBase
    {
        private readonly DesktopApi captureLogic;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";

            captureLogic = new DesktopApi();
        }

        public override void Execute(object parameter, Action<CommandResult> callback)
        {
            var photos = captureLogic.CaptureDesktop();

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
