using Common;
using Common.Extensions;
using System;
using static StatusPlugin.Native.kernel32;

namespace StatusPlugin.Statuses
{
    public class Uptime : IStatus
    {
        public string GetStatus()
        {
            return $"*Uptime*: {GetUptime()}";
        }

        private string GetUptime()
        {
            long tickCount = GetTickCount64();
            return TimeSpan.FromMilliseconds(tickCount).ToReadable();
        }
    }
}
