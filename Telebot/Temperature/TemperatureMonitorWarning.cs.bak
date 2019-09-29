using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Telebot.DeviceProviders;

namespace Telebot.Temperature
{
    public class TemperatureMonitorWarning : TemperatureMonitorBase
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public TemperatureMonitorWarning(params IDeviceProvider[][] deviceProviders)
        {
            foreach (IDeviceProvider[] devices in deviceProviders)
            {
                this.deviceProviders.AddRange(devices);
            }

            CPU_TEMPERATURE_WARNING = Program.appSettings.CPUTemperature;
            GPU_TEMPERATURE_WARNING = Program.appSettings.GPUTemperature;

            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Elapsed += Elapsed;

            IsActive = Program.appSettings.TempMonEnabled;
        }

        ~TemperatureMonitorWarning()
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
                var sensors = deviceProvider.GetTemperatureSensors();
                SensorInfo package = sensors.ElementAt(0);
                float temperature = package.Value;

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

            var parameter = new TemperatureChangedArgs
            {
                Devices = result
            };

            RaiseTemperatureChanged(parameter);
        }
    }
}
