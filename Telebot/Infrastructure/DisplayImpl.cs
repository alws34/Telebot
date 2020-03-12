using Telebot.Common;
using static Telebot.Native.user32;

namespace Telebot.Infrastructure
{
    public class DisplayImpl
    {
        public void SetDisplayOn()
        {
            SetDisplayInState(DisplayState.DisplayStateOn);
        }

        public void SetDisplayOff()
        {
            SetDisplayInState(DisplayState.DisplayStateOff);
        }

        private static void SetDisplayInState(DisplayState state)
        {
            PostMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }
    }
}
