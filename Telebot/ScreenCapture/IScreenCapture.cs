using System;

namespace Telebot.ScreenCapture
{
    public interface IScreenCapture
    {
        event EventHandler<ScreenCaptureArgs> ScreenCaptured;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
