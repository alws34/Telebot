using CPUID.Contracts;
using CPUID.Models;
using System;
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
            foreach (IDevice device in devices)
            {
                Sensor sensor = device.GetSensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);

                switch (device.DeviceClass)
                {
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        if (sensor.Value >= CPU_TEMPERATURE_WARNING)
                        {
                            var args = new TemperatureChangedArgs
                            {
                                DeviceName = device.DeviceName,
                                Temperature = sensor.Value
                            };

                            RaiseTemperatureChanged(args);
                        }
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        if (sensor.Value >= GPU_TEMPERATURE_WARNING)
                        {
                            var args = new TemperatureChangedArgs
                            {
                                DeviceName = device.DeviceName,
                                Temperature = sensor.Value
                            };

                            RaiseTemperatureChanged(args);
                        }
                        break;
                }
            }
        }

        public override void Start()
        {
            base.Start();
            Elapsed(this, null);
        }
    }
}
