using static Telebot.Native.user32;

namespace Telebot.Infrastructure.Apis
{
    public class DisplayApi : IApi
    {
        private readonly DisplayState state;

        public DisplayApi(DisplayState state)
        {
            this.state = state;

            Action = SetDisplayInState;
        }

        public void SetDisplayInState()
        {
            PostMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, (int)state);
        }
    }
}
