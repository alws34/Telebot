using System;
using System.Collections.Generic;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Extensions;
using Telebot.Helpers;
using Telebot.Models;

namespace Telebot.Commands
{
    public class CaptureCmd : ICommand
    {
        public string Name => "/capture";

        public string Description => "Get screenshot of bot and desktop.";

        public event EventHandler<CommandResult> Completed;

        private readonly CaptureController capController;

        public CaptureCmd()
        {
            capController = Program.container.GetInstance<CaptureController>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var bitmaps = new List<Bitmap>
            {
                capController.CaptureControl(cmdInfo.Form1),
                capController.CaptureDesktop()
            };

            foreach (Bitmap bitmap in bitmaps)
            {
                var info = new CommandResult
                {
                    Message = cmdInfo.Message,
                    Stream = bitmap.ToStream(),
                    SendType = SendType.Photo
                };

                Completed?.Invoke(this, info);
            }
        }

        public override string ToString()
        {
            return $"*{Name}* - {Description}";
        }
    }
}
