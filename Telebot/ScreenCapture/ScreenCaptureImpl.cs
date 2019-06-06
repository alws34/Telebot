using System;
using System.Drawing;
using System.Timers;
using Telebot.BusinessLogic;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureImpl : IScreenCapture
    {
        private readonly Timer timer;
        private DateTime stopTime;

        public bool IsActive { get { return timer.Enabled; } }

        public event EventHandler<ScreenCaptureArgs> ScreenCaptured;

        private readonly CaptureLogic captureLogic;

        public ScreenCaptureImpl()
        {
            captureLogic = new CaptureLogic();

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

            var desktops = captureLogic.CaptureDesktop();

            foreach (Bitmap desktop in desktops)
            {
                var result = new ScreenCaptureArgs
                {
                    Capture = desktop
                };

                ScreenCaptured?.Invoke(this, result);
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
