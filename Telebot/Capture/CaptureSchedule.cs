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

        public void Start(int duration_in_sec, int interval_in_sec)
        {
            if (Active)
            {
                RaiseFeedback("Screen capture has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(duration_in_sec);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(interval_in_sec).Seconds()
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
