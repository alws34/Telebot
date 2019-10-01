using System.Runtime.InteropServices;

namespace Telebot.Helpers
{
    public static class Kernel32Helper
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();
    }
}
