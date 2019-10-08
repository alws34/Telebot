using System;
using Telebot.Common;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public abstract class ScreenCaptureBase : IJob<ScreenCaptureArgs>
    {
        public JobType JobType { get; protected set; }

        public bool IsActive { get; protected set; }

        public event EventHandler<ScreenCaptureArgs> Update;

        public abstract void Start();

        public abstract void Stop();

        protected void RaiseScreenCaptured(ScreenCaptureArgs args)
        {
            Update?.Invoke(this, args);
        }
    }
}
