using System;
using System.Drawing;
using Telebot.BusinessLogic;
using Telebot.Extensions;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : CommandBase
    {
        private readonly CaptureLogic captureLogic;

        public CaptureCmd()
        {
            Pattern = "/capture";
            Description = "Get a screenshot of the workstation.";
            captureLogic = Program.container.GetInstance<CaptureLogic>();
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
