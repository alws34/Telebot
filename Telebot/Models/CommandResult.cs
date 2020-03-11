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
        public string Text { get; set; }
        public Stream Stream { get; set; }
        public ResultType ResultType { get; set; }
    }
}
