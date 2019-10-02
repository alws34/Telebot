using FluentScheduler;
using System;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Infrastructure;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureSchedule : IScreenCapture, IScheduledJob
    {
        private DateTime timeStop;

        public bool IsActive { get; private set; }

        public event EventHandler<ScreenCaptureArgs> ScreenCaptured;

        private readonly DesktopApi captureLogic;

        public ScreenCaptureSchedule()
        {
            captureLogic = new DesktopApi();
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
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
            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(interval.Seconds).Seconds()
            );

            IsActive = true;
        }

        public void Stop()
        {
            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
