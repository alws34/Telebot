using CPUID.Contracts;
using CPUID.Models;
using FluentScheduler;
using static CPUID.CPUIDSDK;
using static Telebot.Settings.SettingsFactory;

namespace Telebot.Temperature
{
    public class TempMonWarning : TempMonBase, Settings.IProfile
    {
        private float CPU_TEMPERATURE_WARNING = 65.0f;
        private float GPU_TEMPERATURE_WARNING = 65.0f;

        public TempMonWarning(IDevice[] devices)
        {
            JobType = Common.JobType.Fixed;

            SettingsBase.AddProfile(this);

            this.devices.AddRange(devices);

            if (!Program.isFirstRun)
            {
                CPU_TEMPERATURE_WARNING = MonitorSettings.GetCPUWarningLevel();
                GPU_TEMPERATURE_WARNING = MonitorSettings.GetGPUWarningLevel();
            }

            IsActive = MonitorSettings.GetTempMonitorStatus();

            if (IsActive)
            {
                Start();
            }
        }

        private void Elapsed()
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
            JobManager.AddJob(
                Elapsed,
                (s) => s.WithName(GetType().Name).ToRunNow().AndEvery(7).Seconds()
            );

            IsActive = true;
        }

        public override void Stop()
        {
            JobManager.RemoveJob(GetType().Name);

            IsActive = false;
        }

        public void SaveChanges()
        {
            MonitorSettings.SaveTempMonitorStatus(IsActive);
            MonitorSettings.SaveCPUWarningLevel(CPU_TEMPERATURE_WARNING);
            MonitorSettings.SaveGPUWarningLevel(GPU_TEMPERATURE_WARNING);
        }
    }
}
