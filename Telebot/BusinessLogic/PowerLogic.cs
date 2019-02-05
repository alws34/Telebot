using System.Diagnostics;
using System.Windows.Forms;

namespace Telebot.BusinessLogic
{
    public class PowerLogic
    {
        public void ShutdownWindows()
        {
            Process.Start
            (
                new ProcessStartInfo("shutdown", "/s /t 0")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            );
        }

        public void RestartWindows()
        {
            Process.Start
            (
                new ProcessStartInfo("shutdown", "/r /t 0")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            );
        }

        public void SleepWindows()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }
    }
}
