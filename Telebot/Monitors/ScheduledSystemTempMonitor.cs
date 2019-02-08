using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.Monitors
{
    public class ScheduledSystemTempMonitor : IScheduledTemperatureMonitor
    {
        private readonly Timer workerTimer;
        private DateTime stopTime;
        private readonly IEnumerable<ITemperatureProvider> tempProviders;

        public bool IsActive { get { return workerTimer.Enabled; } }

        public event EventHandler<IHardwareInfo> TemperatureChanged;

        public ScheduledSystemTempMonitor()
        {
            tempProviders = Program.container.GetAllInstances<ITemperatureProvider>();

            workerTimer = new Timer();
            workerTimer.Elapsed += WorkerTimer_Elapsed;
        }

        private void WorkerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (System.DateTime.Now >= stopTime)
            {
                workerTimer.Stop();
                return;
            }

            foreach (ITemperatureProvider tempProvider in tempProviders)
            {
                var hwInfos = tempProvider.GetTemperature();

                foreach (IHardwareInfo hwInfo in hwInfos)
                {
                    TemperatureChanged?.Invoke(this, hwInfo);
                }
            }
        }

        public void Start(int durationInSec, int intervalInSec)
        {
            stopTime = DateTime.Now.AddSeconds(durationInSec);
            workerTimer.Interval = intervalInSec * 1000;
            workerTimer.Start();
        }

        public void Stop()
        {
            workerTimer.Stop();
        }
    }
}
