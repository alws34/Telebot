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

        public event EventHandler<ScreenCaptureArgs> PhotoCaptured;

        private readonly CaptureLogic captureLogic;

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

            var photos = captureLogic.CaptureDesktop();

            foreach (Bitmap photo in photos)
            {
                var result = new ScreenCaptureArgs
                {
                    Photo = photo
                };

                PhotoCaptured?.Invoke(this, result);
            }
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            stopTime = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
