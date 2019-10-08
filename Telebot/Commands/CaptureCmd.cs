using System;
using System.Drawing;
using System.Threading.Tasks;
using Telebot.CoreApis;
using Telebot.Extensions;
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

        public async override void Execute(object parameter, Func<CommandResult, Task> callback)
        {
            var photos = desktopApi.CaptureDesktop();

            foreach (Bitmap photo in photos)
            {
                var result = new CommandResult
                {
                    Stream = photo.ToStream(),
                    SendType = SendType.Photo
                };

                await callback(result);
            }
        }
    }
}
