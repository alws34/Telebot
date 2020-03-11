using FluentScheduler;
using System;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Infrastructure;

namespace Telebot.ScreenCapture
{
    public class ScreenCaptureSchedule : ScreenCaptureBase, IScheduledJob
    {
        private DateTime timeStop;

        private readonly DesktopApi desktopApi;

        public ScreenCaptureSchedule()
        {
            JobType = Common.JobType.Scheduled;

            desktopApi = new DesktopApi();
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
                return;
            }

            var desktops = desktopApi.CaptureDesktop();

            foreach (Bitmap desktop in desktops)
            {
                var result = new ScreenCaptureArgs
                {
                    Capture = desktop
                };

                RaiseScreenCaptured(result);
            }
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            int seconds = Convert.ToInt32(interval.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(seconds).Seconds()
            );

            IsActive = true;
        }

        public override void Stop()
        {
            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
