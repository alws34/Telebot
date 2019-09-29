using System;
using System.Diagnostics;
using Telebot.Extensions;

namespace Telebot.Infrastructure
{
    public class SystemLogic
    {
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
