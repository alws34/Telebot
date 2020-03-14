using Common;
using System;
using Telebot.Common;
using Telebot.Contracts;

namespace Telebot.Capture
{
    public abstract class BaseCapture : INotifyable, IJob<CaptureArgs>
    {
        public JobType JobType { get; protected set; }

        public bool IsActive { get; protected set; }

        public event EventHandler<CaptureArgs> Update;

        public abstract void Start();

        public abstract void Stop();

        protected void RaiseUpdate(CaptureArgs args)
        {
            Update?.Invoke(this, args);
        }
    }
}
