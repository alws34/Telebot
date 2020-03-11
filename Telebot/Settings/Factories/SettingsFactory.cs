namespace Telebot.Settings
{
    public class SettingsFactory
    {
        public ISettings Handler { get; }
        public MainViewSettings MainView { get; }
        public TelegramSettings Telegram { get; }
        public MonitorSettings WarnMon { get; }

        public SettingsFactory()
        {
            Handler = new IniFileSettings();
            MainView = new MainViewSettings(Handler);
            Telegram = new TelegramSettings(Handler);
            WarnMon = new MonitorSettings(Handler);
        }
    }
}
