using System;
using System.Drawing;

namespace Telebot.ScreenCaptures
{
    public interface IScheduledScreenCapture
    {
        bool IsActive { get; }
        void Start(TimeSpan duration, TimeSpan interval, Action<Bitmap> callback);
        void Stop();
    }
}
