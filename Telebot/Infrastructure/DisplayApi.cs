﻿using static Telebot.Native.User32;

namespace Telebot.Infrastructure
{
    public enum DisplayState
    {
        DisplayStateOn = -1,
        DisplayStateOff = 2
    }

    public class DisplayApi
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
