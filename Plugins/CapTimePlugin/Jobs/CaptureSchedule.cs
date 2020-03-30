﻿using CapTimePlugin.Core;
using Contracts.Jobs;
using FluentScheduler;
using System;
using System.Drawing;

namespace Telebot.Capture
{
    public class CaptureSchedule : IJob<CaptureArgs>, IScheduled
    {
        private DateTime timeStop;

        private void Elapsed()
        {
            if (DateTime.Now >= timeStop)
            {
                Stop();
                return;
            }

            var api = new DesktopApi();

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

        public void Start(int DurationSec, int IntervalSec)
        {
            if (Active)
            {
                RaiseFeedback("Screen capture has already been scheduled.");
                return;
            }

            timeStop = DateTime.Now.AddSeconds(DurationSec);

            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(IntervalSec).Seconds()
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
