using Common.Enums;
using System.IO;

namespace Common.Models
{

    public class Response
    {
        public string Text { get; }
        public Stream Raw { get; }
        public int MessageId { get; }
        public ResultType ResultType { get; }

        public Response(string text, int messageId = 0)
        {
            Text = text;
            MessageId = messageId;
            ResultType = ResultType.Text;
        }

        public Response(MemoryStream raw, int messageId = 0)
        {
            Raw = raw;
            MessageId = messageId;
            ResultType = ResultType.Photo;
        }

        public Response(FileStream raw, int messageId = 0)
        {
            Raw = raw;
            MessageId = messageId;
            ResultType = ResultType.Document;
        }
    }
}
