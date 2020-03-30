using Contracts;

namespace Telebot.AppSettings
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

            int.TryParse(idStr, out int result);

            return result;
        }
    }
}
