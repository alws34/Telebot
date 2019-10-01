using System;
using Telebot.Extensions;
using static Telebot.Helpers.Kernel32Helper;

namespace Telebot.Infrastructure
{
    public class SystemLogic
    {
        public string GetUptime()
        {
            long tickCount = GetTickCount();
            return TimeSpan.FromMilliseconds(tickCount).ToReadable();
        }
    }
}
