using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.DeviceProviders;

namespace Telebot.Temperature
{
    public class WarningTempMonitor : TemperatureMonitorBase
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public static ITemperatureMonitor Instance { get; } = new WarningTempMonitor();

        private WarningTempMonitor()
        {
            CPU_TEMPERATURE_WARNING = Program.appSettings.CPUTemperature;
            GPU_TEMPERATURE_WARNING = Program.appSettings.GPUTemperature;

            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Elapsed += Elapsed;

            IsActive = Program.appSettings.TempMonEnabled;
        }

        ~WarningTempMonitor()
        {
            Program.appSettings.TempMonEnabled = IsActive;
            Program.appSettings.CPUTemperature = CPU_TEMPERATURE_WARNING;
            Program.appSettings.GPUTemperature = GPU_TEMPERATURE_WARNING;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = new List<IDeviceProvider>();

            foreach (IDeviceProvider deviceProvider in deviceProviders)
            {
                var temperature = deviceProvider.GetTemperature();

                switch (deviceProvider.DeviceClass)
                {
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        if (temperature >= CPU_TEMPERATURE_WARNING)
                            result.Add(deviceProvider);
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        if (temperature >= GPU_TEMPERATURE_WARNING)
                            result.Add(deviceProvider);
                        break;
                }
            }

            OnTemperatureChanged(result);
        }
    }
}
