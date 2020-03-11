using System.Threading.Tasks;
using Telebot.Models;
using Telegram.Bot.Types.Enums;

namespace Telebot.Clients
{
    public class TransmitText : ITransmitter
    {
        protected readonly IBotClient client;

        public TransmitText(IBotClient client)
        {
            this.client = client;
        }

        public Task Transmit(CommandResult data)
        {
            return client.SendTextMessageAsync(
                data.ChatId,
                data.Text.TrimEnd(),
                parseMode: ParseMode.Markdown,
                replyToMessageId: data.MsgId
            );
        }
    }
}
