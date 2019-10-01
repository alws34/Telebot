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

        public void SaveBotToken(string token)
        {
            settings.WriteString("Telegram", "Token", token);
        }

        public int GetAdminId()
        {
            string idStr = settings.ReadString("Telegram", "AdminId");

            int result = 0;

            int.TryParse(idStr, out result);

            return result;
        }

        public void SaveAdminId(int id)
        {
            string idStr = Convert.ToString(id);

            settings.WriteString("Telegram", "Token", idStr);
        }
    }
}
