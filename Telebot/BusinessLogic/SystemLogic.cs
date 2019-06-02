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
                        strBuilder.AppendLine($"*CPU Usage*: {provider.GetUtilization()}%");
                        strBuilder.AppendLine($"*CPU Temp*: {provider.GetTemperature()}°C");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DRIVE:
                        strBuilder.AppendLine($"*{provider.DeviceName} Used*: {provider.GetUtilization()}%");
                        break;
                    case CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                        var arr = provider.DeviceName.Split(' ');
                        strBuilder.AppendLine($"*GPU {arr[0]}*: {provider.GetTemperature()}°C");
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
