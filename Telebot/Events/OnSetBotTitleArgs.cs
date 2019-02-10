namespace Telebot.Events
{
    public class OnSetBotTitleArgs : IApplicationEvent
    {
        public OnSetBotTitleArgs(string botTitle)
        {
            BotTitle = botTitle;
        }
        public string BotTitle { get; private set; }
    }
}
