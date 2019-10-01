using System;

namespace Telebot.Settings
{
    public class TelegramSettings
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

            int result = 0;

            int.TryParse(idStr, out result);

            return result;
        }
    }
}
