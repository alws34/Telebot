using System;

namespace Telebot.ScreenCaptures
{
    public interface IScreenCapture
    {
        event EventHandler<ScreenCaptureArgs> PhotoCaptured;
        bool IsActive { get; }
        void Start(TimeSpan duration, TimeSpan interval);
        void Stop();
    }
}
