using AppSettings.Contracts;

namespace AppSettings.Singletons
{
    public sealed class GlobalSettings
    {
        public ISettings Main { get; }
        public TelegramSettings Telegram { get; }
        public TempSettings Temperature { get; }

        public static GlobalSettings Instance { get; } = new GlobalSettings();

        GlobalSettings()
        {
            Main = new IniFileHandler();
            Telegram = new TelegramSettings(Main);
            Temperature = new TempSettings(Main);
        }
    }
}
