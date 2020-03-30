using Contracts.Core;
using Enums;
using System.Diagnostics;
using static PowerPlugin.Native.powrprof;
using static PowerPlugin.Native.user32;

namespace PowerPlugin.Core
{
    public class PowerApi : IApi
    {
        private readonly int timeout;

        public PowerApi(PowerType type, int timeout = 0)
        {
            this.timeout = timeout;

            switch (type)
            {
                case PowerType.Shutdown:
                    Action = ShutdownWorkstation;
                    break;
                case PowerType.Restart:
                    Action = RestartWorkstation;
                    break;
                case PowerType.Sleep:
                    Action = SleepWorkstation;
                    break;
                case PowerType.Logoff:
                    Action = LogoffWorkstation;
                    break;
                case PowerType.Lock:
                    Action = LockWorkstation;
                    break;
            }
        }

        public void ShutdownWorkstation()
        {
            var si = new ProcessStartInfo("shutdown", $"/s /t {timeout}")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(si);
        }

        public void RestartWorkstation()
        {
            var si = new ProcessStartInfo("shutdown", "/r /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(si);
        }

        public void SleepWorkstation()
        {
            SetSuspendState(false, false, false);
        }

        public void LockWorkstation()
        {
            LockWorkStation();
        }

        public void LogoffWorkstation()
        {
            ExitWindowsEx(EWX_LOGOFF, 0);
        }
    }
}
