using System;
using System.Drawing;
using System.Timers;
using Telebot.Infrastructure;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureDurated : IScreenCapture
    {
        private readonly Timer timer;
        private DateTime dtStop;

        public bool IsActive => timer.Enabled;

        public event EventHandler<ScreenCaptureArgs> ScreenCaptured;

        private readonly CaptureLogic captureLogic;

        public ScreenCaptureDurated()
        {
            captureLogic = new CaptureLogic();

            timer = new Timer();
            timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= dtStop)
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
            dtStop = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            timer.Start();
            Elapsed(this, null);
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
