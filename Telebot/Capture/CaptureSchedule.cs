using FluentScheduler;
using System;
using System.Drawing;
using Telebot.Contracts;
using Telebot.Infrastructure.Apis;

namespace Telebot.Capture
{
    public class CaptureSchedule : IJob<CaptureArgs>, IScheduled
    {
        private DateTime timeStop;

        public CaptureSchedule()
        {
            JobType = Common.JobType.Scheduled;
        }

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
                return;
            }

            var api = new DeskBmpApi();

            api.Invoke((screens) =>
            {
                foreach (Bitmap screen in screens)
                {
                    var result = new CaptureArgs
                    {
                        Capture = screen
                    };

                    RaiseUpdate(result);
                }
            });
        }

        public void Start(TimeSpan duration, TimeSpan interval)
        {
            if (Active)
            {
                RaiseFeedback("Screen capture has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(duration.TotalSeconds);

            int seconds = Convert.ToInt32(interval.TotalSeconds);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(seconds).Seconds()
            );

            Active = true;
        }

        public override void Stop()
        {
            if (!Active)
            {
                RaiseFeedback("Screen capture has not been scheduled.");
                return;
            }

            JobManager.RemoveJob(GetType().Name);

            Active = false;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }
    }
}
