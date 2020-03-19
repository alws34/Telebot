using System.Diagnostics;
using System.Windows.Forms;
using Telebot.Common;
using Telebot.Native;

namespace Telebot.Infrastructure.Apis
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
            var args = new ProcessStartInfo("shutdown", $"/s /t {timeout}")
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
            user32.LockWorkStation();
        }

        public void LogoffWorkstation()
        {
            user32.ExitWindowsEx(user32.EWX_LOGOFF, 0);
        }
    }
}
