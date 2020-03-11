using System.Runtime.InteropServices;

namespace Telebot.Native
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount64();
    }
}
