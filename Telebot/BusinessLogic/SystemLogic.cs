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
                        strBuilder.AppendLine($"*RAM Used.*: {provider.GetUtilization()}%");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                        double utilization = Math.Round(provider.GetUtilization(), 1);
                        strBuilder.AppendLine($"*CPU Usage*: {utilization}%");
                        strBuilder.AppendLine($"*CPU Temp*: {provider.GetTemperature()}°C");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DRIVE:
                        var driveBrand = provider.DeviceName.Split(' ')[0];
                        strBuilder.AppendLine($"*{driveBrand} Used*: {provider.GetUtilization()}%");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        var gpuBrand = provider.DeviceName.Split(' ')[0];
                        strBuilder.AppendLine($"*GPU {gpuBrand}*: {provider.GetTemperature()}°C");
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
