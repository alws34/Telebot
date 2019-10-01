using System;

namespace Telebot.Settings
{
    public class TelegramSettings : SettingsBase
    {
        private readonly ISettings settings;

        public TelegramSettings(ISettings settings)
        {
            this.settings = settings;
        }

        public string GetBotToken()
        {
            return settings.ReadString("Telegram", "Token");
        }

        public int GetAdminId()
        {
            string idStr = settings.ReadString("Telegram", "AdminId");

            return Convert.ToInt32(idStr);
        }
    }
}
