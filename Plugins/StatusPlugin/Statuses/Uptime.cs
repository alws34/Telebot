using Common;
using Common.Extensions;
using System;
using static StatusPlugin.Native.kernel32;

namespace StatusPlugin.Statuses
{
    public class Uptime : IClassStatus
    {
        public string GetStatus()
        {
            long tickCount = GetTickCount64();
            string formatted = TimeSpan.FromMilliseconds(tickCount).ToReadable();
            return $"*Uptime*: {formatted}";
        }
    }
}
