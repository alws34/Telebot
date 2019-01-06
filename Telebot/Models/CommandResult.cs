using System.IO;
using Telegram.Bot.Types;

namespace Telebot.Models
{
    public enum SendType
    {
        Text = 0,
        Photo = 1
    };

    public class CommandResult
    {
        public string Text { get; set; }
        public Stream Stream { get; set; }
        public Message Message { get; set; }
        public SendType SendType { get; set; }
    }
}
