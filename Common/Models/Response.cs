using Common.Enums;
using System.IO;

namespace Common.Models
{

    public class Response
    {
        public string Text { get; }
        public Stream Raw { get; }
        public bool Reply { get; }
        public int MessageId { get; }
        public ResultType ResultType { get; }

        public Response(string text, bool reply = true, int messageId = 0)
        {
            Text = text;
            Reply = reply;
            MessageId = messageId;
            ResultType = ResultType.Text;
        }

        public Response(MemoryStream raw, bool reply = true, int messageId = 0)
        {
            Raw = raw;
            Reply = reply;
            MessageId = messageId;
            ResultType = ResultType.Photo;
        }

        public Response(FileStream raw, bool reply = true, int messageId = 0)
        {
            Raw = raw;
            Reply = reply;
            MessageId = messageId;
            ResultType = ResultType.Document;
        }
    }
}
