using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Telebot.DeviceProviders;
using Telebot.Extensions;

namespace Telebot.BusinessLogic
{
    public class SystemLogic
    {
        private readonly List<IDeviceProvider> deviceProviders;

        public SystemLogic()
        {

        }

        public SystemLogic(params IDeviceProvider[][] deviceProvidersArg)
        {
            deviceProviders = new List<IDeviceProvider>();

            foreach (IDeviceProvider[] deviceProvidersArr in deviceProvidersArg)
            {
                deviceProviders.AddRange(deviceProvidersArr);
            }
        }

        public string GetSystemStatus()
        {
            var strBuilder = new StringBuilder();

            foreach(IDeviceProvider deviceProvider in deviceProviders)
            {
                switch (deviceProvider.DeviceClass)
                {
                    case CPUIDSDK.CLASS_DEVICE_MAINBOARD:
                        float ram_util = deviceProvider.GetUtilizationSensors().ElementAt(0).Value;
                        strBuilder.AppendLine($"*RAM Used.*: {ram_util}%");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        float cpu_util = deviceProvider.GetUtilizationSensors().ElementAt(0).Value;
                        double cpu_util_round = Math.Round(cpu_util, 0);
                        strBuilder.AppendLine($"*CPU Usage*: {cpu_util_round}%");
                        float cpu_temp = deviceProvider.GetTemperatureSensors().ElementAt(0).Value;
                        strBuilder.AppendLine($"*CPU Temp*: {cpu_temp}°C");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DRIVE:
                        for (int i = 0; i < deviceProvider.SensorsCount; i++)
                        {
                            var driveSensor = deviceProvider.GetUtilizationSensors().ElementAt(i);
                            string name = driveSensor.Name.ToUpper().Replace("SPACE", "Storage used");
                            double drive_util = Math.Round(driveSensor.Value, 0);
                            strBuilder.AppendLine($"*{name}*: {drive_util}%");
                        }
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        var gpuBrand = deviceProvider.DeviceName.Split(' ')[0];
                        float gpu_temp = deviceProvider.GetTemperatureSensors().ElementAt(0).Value;
                        strBuilder.AppendLine($"*GPU {gpuBrand}*: {gpu_temp}°C");
                        break;
                }
            }

            return strBuilder.ToString().TrimEnd();
        }

        public string GetUptime()
        {
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue();
                return TimeSpan.FromSeconds(uptime.NextValue()).ToReadable();
            }
        }
    }
}
