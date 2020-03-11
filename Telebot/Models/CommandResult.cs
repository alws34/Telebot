using System.IO;

namespace Telebot.Models
{
    public enum ResultType
    {
        Text,
        Photo,
        Document
    };

    public class CommandResult
    {
        public long ChatId { get; set; }
        public long FromId { get; set; }
        public int MsgId { get; set; }
        public string Text { get; set; }
        public Stream Raw { get; set; }
        public ResultType ResultType { get; set; }
    }
}
