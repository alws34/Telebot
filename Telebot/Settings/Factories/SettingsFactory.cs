namespace Telebot.Settings
{
    public class SettingsFactory
    {
        public ISettings Handler { get; }
        public TelegramSettings Telegram { get; }
        public TempSettings Temperature { get; }

        public SettingsFactory()
        {
            Handler = new IniFileSettings();
            Telegram = new TelegramSettings(Handler);
            Temperature = new TempSettings(Handler);
        }
    }
}
