using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Telebot.Helpers.User32Helper;

namespace Telebot.CoreApis
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
                const uint WS_CAPTION = 0x00C00000;
                const uint WS_VISIBLE = 0x10000000;

                IntPtr styles = GetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE);

                long value = styles.ToInt64();

                return isBitSet(value, WS_CAPTION) && isBitSet(value, WS_VISIBLE);
            }

            string GetWndText(IntPtr hWnd)
            {
                int length = GetWindowTextLength(hWnd);

                var sb = new StringBuilder(length + 1);

                GetWindowText(hWnd, sb, sb.Capacity);

                return sb.ToString();
            }

            bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
            {
                if (isTopLevelWindow(hWnd))
                {
                    string wndCaption = GetWndText(hWnd);

                    uint wndPid;
                    GetWindowThreadProcessId(hWnd, out wndPid);

                    builder.AppendLine($"- {wndCaption} ({wndPid})");
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
                    string name = process.MainModule.FileVersionInfo.ProductName;
                    int pid = process.Id;
                    result.AppendLine($"- {name} ({pid})");
                }
                catch
                {

                }
            }

            return result.ToString().TrimEnd();
        }
    }
}
