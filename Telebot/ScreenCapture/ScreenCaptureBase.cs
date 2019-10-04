using System;

namespace Telebot.ScreenCapture
{
    public abstract class ScreenCaptureBase : IScreenCapture
    {
        public ScreenCapType ScreenCapType { get; protected set; }

        public bool IsActive { get; protected set; }

        public event EventHandler<ScreenCaptureArgs> ScreenCaptured;

        public abstract void Start();

        public abstract void Stop();

        protected void RaiseScreenCaptured(ScreenCaptureArgs args)
        {
            ScreenCaptured?.Invoke(this, args);
        }
    }
}
