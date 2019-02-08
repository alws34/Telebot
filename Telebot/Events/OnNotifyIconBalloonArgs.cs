namespace Telebot.Events
{
    public class OnNotifyIconBalloonArgs : IApplicationEvent
    {
        public OnNotifyIconBalloonArgs(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
