using System;
using System.Diagnostics;
using Telebot.Extensions;
using static Telebot.Helpers.Kernel32Helper;

namespace Telebot.CoreApis
{
    public class SystemApi
    {
        public void SetBrightness(int percentage)
        {
            string args = $"/C powershell (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods).WmiSetBrightness(1,{percentage})";

            var si = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "CMD.exe",
                Arguments = args
            };

            Process.Start(si);
        }

        public string GetUptime()
        {
            long tickCount = GetTickCount64();
            return TimeSpan.FromMilliseconds(tickCount).ToReadable();
        }
    }
}
