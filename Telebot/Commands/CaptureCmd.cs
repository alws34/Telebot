using System;
using System.Collections.Generic;
using System.Drawing;
using Telebot.Contracts;
using Telebot.BusinessLogic;
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

        private readonly CaptureLogic captureLogic;
        private readonly Form1 form1;

        public CaptureCmd()
        {
            captureLogic = Program.container.GetInstance<CaptureLogic>();
            form1 = Program.container.GetInstance<Form1>();
        }

        public void Execute(object parameter)
        {
            var cmdInfo = parameter as CommandInfo;

            var bitmaps = new List<Bitmap>
            {
                captureLogic.CaptureControl(form1),
                captureLogic.CaptureDesktop()
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
