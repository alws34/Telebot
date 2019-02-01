namespace Telebot.Events
{
    public class ShowNotifyIconBalloon : IApplicationEvent
    {
        public ShowNotifyIconBalloon(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
