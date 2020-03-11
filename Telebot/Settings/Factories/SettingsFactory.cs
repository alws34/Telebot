namespace Telebot.Settings
{
    public class SettingsFactory
    {
        public ISettings Handler { get; }
        public TelegramSettings Telegram { get; }
        public MonitorSettings WarnMon { get; }

        public SettingsFactory()
        {
            Handler = new IniFileSettings();
            Telegram = new TelegramSettings(Handler);
            WarnMon = new MonitorSettings(Handler);
        }
    }
}
