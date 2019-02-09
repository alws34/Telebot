using System;
using System.Drawing;
using System.Timers;
using Telebot.BusinessLogic;

namespace Telebot.ScheduledOperations
{
    public class ScheduledScreenCapture : IScheduledScreenCapture
    {
        private readonly Timer workerTimer;
        private DateTime stopTime;

        public bool IsActive { get { return workerTimer.Enabled; } }

        public event EventHandler<Bitmap> Captured;

        private readonly CaptureLogic captureLogic;

        public ScheduledScreenCapture()
        {
            captureLogic = Program.container.GetInstance<CaptureLogic>();

            workerTimer = new Timer();
            workerTimer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= stopTime)
            {
                workerTimer.Stop();
                return;
            }

            Captured?.Invoke(this, captureLogic.CaptureDesktop());
        }

        public void Start(int durationInSec, int intervalInSec)
        {
            stopTime = DateTime.Now.AddSeconds(durationInSec);
            workerTimer.Interval = intervalInSec * 1000;
            workerTimer.Start();
        }

        public void Stop()
        {
            workerTimer.Stop();
        }
    }
}
