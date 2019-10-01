namespace Telebot.Settings
{
    public static class SettingsFactory
    {
        public static SettingsBase SettingsBase { get; }
        public static GuiSettings GuiSettings { get; }
        public static TelegramSettings TelegramSettings { get; }
        public static MonitorSettings MonitorSettings { get; }

        static SettingsFactory()
        {
            SettingsBase = new SettingsBase();
            GuiSettings = new GuiSettings(SettingsBase);
            TelegramSettings = new TelegramSettings(SettingsBase);
            MonitorSettings = new MonitorSettings(SettingsBase);
        }
    }
}
