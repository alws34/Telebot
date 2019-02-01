using Telebot.Commands;
using Telegram.Bot.Types;

namespace Telebot.Models
{
    public class CommandInfo
    {
        public Message Message { get; set; }
        public ICommand[] Commands { get; set; }
    }
}
