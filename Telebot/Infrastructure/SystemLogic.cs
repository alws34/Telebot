using System;
using System.Diagnostics;
using Telebot.Extensions;
using static Telebot.Helpers.Kernel32Helper;

namespace Telebot.Infrastructure
{
    public class SystemLogic
    {
        public void SetBrightness(int percentage)
        {
            string args = $"/C powershell (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods).WmiSetBrightness(1,{percentage})";

            ProcessStartInfo si = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "CMD.exe",
                Arguments = args
            };

            Process.Start(si);
        }

        public string GetUptime()
        {
            long tickCount = GetTickCount();
            return TimeSpan.FromMilliseconds(tickCount).ToReadable();
        }
    }
}
