using System;

namespace Telebot.ScreenCapture
{
    public enum ScreenCapType
    {
        Scheduled
    }

    public interface IScreenCapture
    {
        event EventHandler<ScreenCaptureArgs> ScreenCaptured;
        ScreenCapType ScreenCapType { get; }
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
