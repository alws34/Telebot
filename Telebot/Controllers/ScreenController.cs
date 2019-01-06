using static Telebot.Helpers.User32Helper;

namespace Telebot.Controllers
{
    public enum MonitorState
    {
        MonitorStateOn = -1,
        MonitorStateOff = 2
    }

    public class ScreenController
    {
        public void SetMonitorOn()
        {
            SetMonitorInState(MonitorState.MonitorStateOn);
        }

        public void SetMonitorOff()
        {
            SetMonitorInState(MonitorState.MonitorStateOff);
        }

        private static void SetMonitorInState(MonitorState state)
        {
            PostMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }
    }
}
