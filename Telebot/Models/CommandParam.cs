using System.Text.RegularExpressions;
using Telebot.Commands;
using Telegram.Bot.Types;

namespace Telebot.Models
{
    public class CommandParam
    {
        public Message Message { get; set; }
        public ICommand[] Commands { get; set; }
        public Match Parameters { get; set; }
    }
}
