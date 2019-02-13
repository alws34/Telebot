using System;
using System.Drawing;
using System.Timers;
using Telebot.BusinessLogic;

namespace Telebot.ScreenCaptures
{
    public class ScheduledScreenCapture : IScheduledScreenCapture
    {
        private readonly Timer timer;
        private DateTime stopTime;

        public static IScheduledScreenCapture Instance { get; } = new ScheduledScreenCapture();

        public bool IsActive { get { return timer.Enabled; } }

        private readonly CaptureLogic captureLogic;

        private Action<Bitmap> callback;

        ScheduledScreenCapture()
        {
            captureLogic = Program.container.GetInstance<CaptureLogic>();

            timer = new Timer();
            timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= stopTime)
            {
                timer.Stop();
                return;
            }

            callback(captureLogic.CaptureDesktop());
        }

        public void Start(TimeSpan duration, TimeSpan interval, Action<Bitmap> callback)
        {
            stopTime = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            this.callback = callback;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
