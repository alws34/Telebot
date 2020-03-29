using System.Runtime.InteropServices;

namespace StatusPlugin.Native
{
    public static class kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount64();
    }
}
