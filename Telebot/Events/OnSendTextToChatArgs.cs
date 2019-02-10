namespace Telebot.Events
{
    public class OnSendTextToChatArgs : IApplicationEvent
    {
        public OnSendTextToChatArgs(string text, long chatid, int messageid)
        {
            Text = text;
            ChatId = chatid;
            MessageId = messageid;
        }
        public string Text { get; private set; }
        public long ChatId { get; private set; }
        public int MessageId { get; private set; }
    }
}
