using System.Runtime.InteropServices;

namespace PowerPlugin.Native
{
    public static partial class user32
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("user32.dll")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
    }

    public static partial class user32
    {
        public const int EWX_LOGOFF = 0;
    }
}
