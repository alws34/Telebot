using System;
using System.Timers;

namespace Telebot.Temperature
{
    public class TimedTempMonitor : TemperatureMonitorBase
    {
        private DateTime dtStop;

        public static ITemperatureMonitor Instance { get; } = new TimedTempMonitor();

        TimedTempMonitor()
        {
            timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= dtStop)
            {
                timer.Stop();
                return;
            }

            OnTemperatureChanged(deviceProviders);
        }

        public override void Start(TimeSpan duration, TimeSpan interval)
        {
            dtStop = DateTime.Now.AddSeconds(duration.TotalSeconds);
            timer.Interval = interval.TotalMilliseconds;
            Start();
        }
    }
}
