namespace Telebot.Events
{
    public class OnNotifyIconVisibilityArgs : IApplicationEvent
    {
        public OnNotifyIconVisibilityArgs(bool visible)
        {
            Visible = visible;
        }

        public bool Visible { get; private set; }
    }
}
