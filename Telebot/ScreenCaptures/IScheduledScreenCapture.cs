using System;
using System.Drawing;

namespace Telebot.ScreenCaptures
{
    public interface IScheduledScreenCapture
    {
        event EventHandler<ScreenCaptureArgs> PhotoCaptured;
        bool IsActive { get; }
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}
