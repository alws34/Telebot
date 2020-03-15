using FluentScheduler;
using System;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Infrastructure;

namespace Telebot.Capture
{
    public class CaptureSchedule : ICapture, IScheduled
    {
        private DateTime timeStop;

        private readonly ScreenImpl screen;

        public CaptureSchedule()
        {
            JobType = Common.JobType.Scheduled;

            screen = new ScreenImpl();
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
                return;
            }

            var screens = screen.CaptureDesktop();

            foreach (Bitmap screen in screens)
            {
                var result = new CaptureArgs
                {
                    Capture = screen
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
            if (!IsActive)
            {
                RaiseNotify("Screen capture has not been scheduled.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
