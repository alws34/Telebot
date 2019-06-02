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
        private readonly IEnumerable<IDeviceProvider> providers;

        public SystemLogic()
        {
            var cpuProviders = ProvidersFactory.GetCPUProviders();
            var gpuProviders = ProvidersFactory.GetGPUProviders();
            var ramProviders = ProvidersFactory.GetRAMProviders();
            var driveProviders = ProvidersFactory.GetDriveProviders();

            providers = ramProviders
                .Concat(cpuProviders)
                .Concat(driveProviders)
                .Concat(gpuProviders);
        }

        public string GetSystemStatus()
        {
            var strBuilder = new StringBuilder();

            foreach(IDeviceProvider provider in providers)
            {
                switch (provider.DeviceClass)
                {
                    case CPUIDSDK.CLASS_DEVICE_MAINBOARD:
                        float ram_util = provider.GetUtilization().ElementAt(0).Value;
                        strBuilder.AppendLine($"*RAM Used.*: {ram_util}%");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        float cpu_util = provider.GetUtilization().ElementAt(0).Value;
                        double cpu_util_round = Math.Round(cpu_util, 0);
                        strBuilder.AppendLine($"*CPU Usage*: {cpu_util_round}%");
                        float cpu_temp = provider.GetTemperature().ElementAt(0).Value;
                        strBuilder.AppendLine($"*CPU Temp*: {cpu_temp}°C");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DRIVE:
                        for (int i = 0; i < provider.SensorsCount; i++)
                        {
                            var driveSensor = provider.GetUtilization().ElementAt(i);
                            string name = driveSensor.Name.ToUpper().Replace("SPACE", "Storage used");
                            double drive_util = Math.Round(driveSensor.Value, 0);
                            strBuilder.AppendLine($"*{name}*: {drive_util}%");
                        }
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        var gpuBrand = provider.DeviceName.Split(' ')[0];
                        float gpu_temp = provider.GetTemperature().ElementAt(0).Value;
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
