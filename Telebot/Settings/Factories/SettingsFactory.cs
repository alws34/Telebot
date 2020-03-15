namespace Telebot.Settings
{
    public class SettingsFactory
    {
        public ISettings Main { get; }
        public TelegramSettings Telegram { get; }
        public TempSettings Temperature { get; }

        public SettingsFactory()
        {
            Main = new IniFileSettings();
            Telegram = new TelegramSettings(Main);
            Temperature = new TempSettings(Main);
        }
    }
}
