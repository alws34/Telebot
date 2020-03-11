using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Telebot.Helpers.User32Helper;

namespace Telebot.Infrastructure
{
    public class WindowsApi
    {
        public string GetForegroundApps()
        {
            var builder = new StringBuilder();

            bool isBitSet(long flags, uint bit)
            {
                return (flags & bit) != 0;
            }

            bool isTopLevelWindow(IntPtr hWnd)
            {
                IntPtr styles = GetWindowLong(hWnd, (int)GWL.GWL_STYLE);

                long value = styles.ToInt64();

                return isBitSet(value, WS_CAPTION) && isBitSet(value, WS_VISIBLE);
            }

            bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
            {
                if (isTopLevelWindow(hWnd))
                {
                    GetWindowThreadProcessId(hWnd, out uint pid);

                    var process = Process.GetProcessById((int)pid);

                    var fi = process.MainModule.FileVersionInfo;

                    string name = fi.FileDescription;

                    if (string.IsNullOrEmpty(name))
                        name = fi.ProductName;

                    builder.AppendLine($"- {name} ({pid})");
                }

                return true;
            }

            EnumWindows(EnumWindowProc, IntPtr.Zero);

            return builder.ToString();
        }

        public string GetBackgroundApps()
        {
            var result = new StringBuilder();

            var processes = Process.GetProcesses().Where(x => x.SessionId != 0);

            foreach (Process process in processes)
            {
                try
                {
                    var fi = process.MainModule.FileVersionInfo;

                    string name = fi.FileDescription;

                    int pid = process.Id;

                    result.AppendLine($"- {name} ({pid})");
                }
                catch
                {

                }
            }

            return result.ToString();
        }
    }
}
