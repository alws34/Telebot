﻿using System.Diagnostics;
using System.Windows.Forms;
using Telebot.Helpers;

namespace Telebot.Infrastructure
{
    public class PowerLogic
    {
        public void ShutdownWorkstation()
        {
            var args = new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(args);
        }

        public void ShutdownWorkstation(int seconds)
        {
            var args = new ProcessStartInfo("shutdown", $"/s /t {seconds}")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(args);
        }

        public void RestartWorkstation()
        {
            var args = new ProcessStartInfo("shutdown", "/r /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(args);
        }

        public void SleepWorkstation()
        {
            Application.SetSuspendState(PowerState.Suspend, false, false);
        }

        public void LockWorkstation()
        {
            User32Helper.LockWorkStation();
        }

        public void LogoffWorkstation()
        {
            User32Helper.ExitWindowsEx(User32Helper.EWX_LOGOFF, 0);
        }
    }
}
