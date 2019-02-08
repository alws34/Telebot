using System;
using System.Drawing;

namespace Telebot.ScheduledOperations
{
    public interface IScheduledScreenCapture
    {
        event EventHandler<Bitmap> Captured;
        bool IsActive { get; }
        void Start(int durationInSec, int intervalInSec);
        void Stop();
    }
}
