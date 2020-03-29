namespace Telebot.Settings
{
    public class AppSettings
    {
        public ISettings Main { get; }
        public TelegramSettings Telegram { get; }
        public TempSettings Temperature { get; }

        public AppSettings()
        {
            Main = new IniFileHandler();
            Telegram = new TelegramSettings(Main);
            Temperature = new TempSettings(Main);
        }
    }
}
