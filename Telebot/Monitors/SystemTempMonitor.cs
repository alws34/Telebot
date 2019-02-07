using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.Monitors
{
    public class SystemTempMonitor : ITemperatureMonitor
    {
        private const int REFRESH_INTERVAL = 10000;

        private readonly Timer workerTimer;
        private readonly IEnumerable<ITemperatureProvider> tempProviders;
        public bool IsActive { get { return workerTimer.Enabled; } }

        public event EventHandler<IHardwareInfo> TemperatureChanged;

        public SystemTempMonitor()
        {
            tempProviders = Program.container.GetAllInstances<ITemperatureProvider>();

            workerTimer = new Timer(REFRESH_INTERVAL);
            workerTimer.Elapsed += WorkerTimer_Elapsed;
        }

        private void WorkerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (ITemperatureProvider tempProvider in tempProviders)
            {
                var hwInfos = tempProvider.GetTemperature();

                foreach (IHardwareInfo hwInfo in hwInfos)
                {
                    TemperatureChanged?.Invoke(this, hwInfo);
                }
            }
        }

        public void Start()
        {
            workerTimer.Start();
        }

        public void Stop()
        {
            workerTimer.Stop();
        }
    }
}
