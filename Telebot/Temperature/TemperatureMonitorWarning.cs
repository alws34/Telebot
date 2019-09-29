using CPUID.Contracts;
using CPUID.Models;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Telebot.Temperature
{
    public class TemperatureMonitorWarning : TemperatureMonitorBase
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public TemperatureMonitorWarning(params IDevice[][] devicesArr)
        {
            foreach (IDevice[] devices in devicesArr)
            {
                this.devices.AddRange(devices);
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
            var result = new List<IDevice>();

            foreach (IDevice device in devices)
            {
                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                Sensor sensor = sensors[0];

                switch (device.DeviceClass)
                {
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        if (sensor.Value >= CPU_TEMPERATURE_WARNING)
                            result.Add(device);
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        if (sensor.Value >= GPU_TEMPERATURE_WARNING)
                            result.Add(device);
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
