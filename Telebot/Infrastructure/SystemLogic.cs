using System;
using System.Diagnostics;
using Telebot.Extensions;

namespace Telebot.Infrastructure
{
    public class SystemLogic
    {
        private readonly PerformanceCounter sysUpTime;

        public SystemLogic()
        {
            sysUpTime = new PerformanceCounter("System", "System Up Time");
        }

        public string GetUptime()
        {
            sysUpTime.NextValue();
            return TimeSpan.FromSeconds(sysUpTime.NextValue()).ToReadable();
        }
    }
}
