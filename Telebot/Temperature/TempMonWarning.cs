using CPUID.Contracts;
using CPUID.Models;
using System;
using System.Timers;
using static CPUIDSDK;
using static Telebot.Settings.SettingsFactory;

namespace Telebot.Temperature
{
    public class TempMonWarning : TempMonBase, Settings.IProfile
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public TempMonWarning(params IDevice[][] devicesArr)
        {
            foreach (IDevice[] devices in devicesArr)
            {
                this.devices.AddRange(devices);
            }

            CPU_TEMPERATURE_WARNING = MonitorSettings.GetCPUWarningLevel();
            GPU_TEMPERATURE_WARNING = MonitorSettings.GetGPUWarningLevel();

            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Elapsed += Elapsed;

            IsActive = MonitorSettings.GetTempMonitorStatus();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (IDevice device in devices)
            {
                Sensor sensor = device.GetSensor(SENSOR_CLASS_TEMPERATURE);

                switch (device.DeviceClass)
                {
                    case CLASS_DEVICE_PROCESSOR:
                        if (sensor.Value >= CPU_TEMPERATURE_WARNING)
                        {
                            var args = new TempChangedArgs
                            {
                                DeviceName = device.DeviceName,
                                Temperature = sensor.Value
                            };

                            RaiseTemperatureChanged(args);
                        }
                        break;
                    case CLASS_DEVICE_DISPLAY_ADAPTER:
                        if (sensor.Value >= GPU_TEMPERATURE_WARNING)
                        {
                            var args = new TempChangedArgs
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

        public void SaveChanges()
        {
            MonitorSettings.SaveTempMonitorStatus(IsActive);
            MonitorSettings.SaveCPUWarningLevel(CPU_TEMPERATURE_WARNING);
            MonitorSettings.SaveGPUWarningLevel(GPU_TEMPERATURE_WARNING);
        }
    }
}
