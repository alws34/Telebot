using System;
using System.Drawing;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureArgs : EventArgs
    {
        public Bitmap Capture { get; set; }
    }
}
