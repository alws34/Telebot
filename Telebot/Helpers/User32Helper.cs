using System;
using System.Runtime.InteropServices;

namespace Telebot.Helpers
{
    public static class User32Helper
    {
        public const int HWND_BROADCAST = 0xFFFF;
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MONITORPOWER = 0xF170;

        [DllImport("user32.dll")]
        public static extern int PostMessage(int hWnd, int hMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public const int EWX_LOGOFF = 0;

        [DllImport("user32.dll")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
    }
}
