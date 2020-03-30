using Common.Enums;
using System.IO;

namespace Common.Models
{

    public class Response
    {
        public string Text { get; }
        public Stream Raw { get; }
        public bool Reply { get; }
        public ResultType ResultType { get; }

        public Response(string text, bool reply = true)
        {
            Text = text;
            Reply = reply;
            ResultType = ResultType.Text;
        }

        public Response(MemoryStream raw, bool reply = true)
        {
            Raw = raw;
            Reply = reply;
            ResultType = ResultType.Photo;
        }

        public Response(FileStream raw, bool reply = true)
        {
            Raw = raw;
            Reply = reply;
            ResultType = ResultType.Document;
        }
    }
}
