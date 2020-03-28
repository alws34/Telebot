using System.IO;
using Telebot.Common;

namespace Telebot.Models
{

    public class Response
    {
        public string Text { get; }
        public Stream Raw { get; }
        public ResultType ResultType { get; }

        public Response(string text)
        {
            Text = text;
            ResultType = ResultType.Text;
        }

        public Response(MemoryStream raw)
        {
            Raw = raw;
            ResultType = ResultType.Photo;
        }

        public Response(FileStream raw)
        {
            Raw = raw;
            ResultType = ResultType.Document;
        }
    }
}
