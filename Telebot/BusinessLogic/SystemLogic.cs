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

        public string GetProvidersInfo()
        {
            var strBuilder = new StringBuilder();

            foreach(IDeviceProvider deviceProvider in deviceProviders)
            {
                strBuilder.AppendLine(deviceProvider.ToString());
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
