using Telegram.Bot;

namespace Telebot.Clients
{
    public interface ITelebotClient : ITelegramBotClient
    {
        int AdminID { get; }
    }
}
