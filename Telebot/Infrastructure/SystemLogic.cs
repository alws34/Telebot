using CPUID.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Telebot.Extensions;

namespace Telebot.Infrastructure
{
    public class SystemLogic
    {
        private readonly List<IDevice> devices;

        public SystemLogic()
        {

        }

        public SystemLogic(params IDevice[][] devicesArg)
        {
            devices = new List<IDevice>();

            foreach (IDevice[] devicesArr in devicesArg)
            {
                devices.AddRange(devicesArr);
            }
        }

        public string GetDevicesInfo()
        {
            var strBuilder = new StringBuilder();

            foreach (IDevice device in devices)
            {
                strBuilder.AppendLine(device.ToString());
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
