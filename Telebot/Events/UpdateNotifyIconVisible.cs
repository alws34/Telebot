namespace Telebot.Events
{
    public class UpdateNotifyIconVisible : IApplicationEvent
    {
        public UpdateNotifyIconVisible(bool visible)
        {
            Visible = visible;
        }

        public bool Visible { get; private set; }
    }
}
