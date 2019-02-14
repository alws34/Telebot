using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.HwProviders;
using Telebot.Models;

namespace Telebot.Monitors
{
    public class PermanentTempMonitor : TemperatureMonitorBase
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public static ITemperatureMonitor Instance { get; } = new PermanentTempMonitor();

        PermanentTempMonitor()
        {
            CPU_TEMPERATURE_WARNING = Program.appSettings.CPUTemperature;
            GPU_TEMPERATURE_WARNING = Program.appSettings.GPUTemperature;

            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Elapsed += Elapsed;
        }

        ~PermanentTempMonitor()
        {
            Program.appSettings.TempMonEnabled = IsActive;
            Program.appSettings.CPUTemperature = CPU_TEMPERATURE_WARNING;
            Program.appSettings.GPUTemperature = GPU_TEMPERATURE_WARNING;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = new List<HardwareInfo>();

            foreach (ITemperatureProvider temperatureProvider in temperatureProviders)
            {
                foreach (HardwareInfo device in temperatureProvider.GetTemperature())
                {
                    switch (device.DeviceClass)
                    {
                        case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                            if (device.Value >= CPU_TEMPERATURE_WARNING)
                            {
                                result.Add(device);
                            }
                            break;
                        case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                            if (device.Value >= GPU_TEMPERATURE_WARNING)
                            {
                                result.Add(device);
                            }
                            break;
                    }
                }
            }

            OnTemperatureChanged(result);
        }
    }
}
