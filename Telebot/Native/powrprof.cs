using System.Runtime.InteropServices;

namespace Telebot.Native
{
    public static class powrprof
    {
        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern uint SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
    }
}
