using FluentScheduler;
using System;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Infrastructure;

namespace Telebot.Capture
{
    public class CaptureSchedule : BaseCapture, IScheduled
    {
        private DateTime timeStop;

        private readonly ScreenImpl desktopApi;

        public CaptureSchedule()
        {
            JobType = Common.JobType.Scheduled;

            desktopApi = new ScreenImpl();
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
                var result = new CaptureArgs
                {
                    Capture = desktop
                };

                RaiseUpdate(result);
            }
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            if (IsActive)
            {
                RaiseNotify("Screen capture has already been scheduled.");
                return;
            }

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
