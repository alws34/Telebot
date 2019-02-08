using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.ScheduledOperations
{
    public class ScheduledSystemTempMonitor : IScheduledTemperatureMonitor
    {
        private readonly Timer workerTimer;
        private DateTime stopTime;
        private readonly IEnumerable<ITemperatureProvider> tempProviders;

        public bool IsActive { get { return workerTimer.Enabled; } }

        public event EventHandler<IEnumerable<HardwareInfo>> TemperatureChanged;

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

            var result = new List<HardwareInfo>();

            foreach (ITemperatureProvider tempProvider in tempProviders)
            {
                result.AddRange(tempProvider.GetTemperature());
            }

            TemperatureChanged?.Invoke(this, result);
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
