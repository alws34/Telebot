using Contracts.Settings;

namespace Telebot.AppSettings
{
    public sealed class GlobalSettings
    {
        public ISettings Main { get; }
        public TelegramSettings Telegram { get; }

        public static GlobalSettings Instance { get; private set; }

        // Early initialization "hack"
        public static void CreateInstance()
        {
            Instance = new GlobalSettings();
        }

        private GlobalSettings()
        {
            Main = new IniFileHandler();
            Telegram = new TelegramSettings(Main);
        }
    }
}
