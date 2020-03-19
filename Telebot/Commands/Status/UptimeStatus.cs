using System;
using Telebot.Extensions;
using static Telebot.Native.kernel32;

namespace Telebot.Commands.Status
{
    public class UptimeStatus : IStatus
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
