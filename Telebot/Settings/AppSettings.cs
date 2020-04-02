using BotSdk.Settings;

namespace Telebot.Settings
{
    public class AppSettings : IAppSettings
    {
        public ISettings Main { get; }
        public TelegramSettings Telegram { get; }

        public AppSettings()
        {
            Main = new IniFileHandler();
            Telegram = new TelegramSettings(Main);
        }
    }

    public interface IAppSettings
    {
        TelegramSettings Telegram { get; }
    }
}
