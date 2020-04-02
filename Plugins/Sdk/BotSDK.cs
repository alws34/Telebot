using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotSDK
{
    public abstract class IModule
    {
        protected ResponseHandler ResultHandler;

        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Execute(Request req);

        public virtual void Initialize(ModuleData data)
        {
            ResultHandler = data.ResultHandler;
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }

    public class ModuleData
    {
        public IServiceProvider IoCProvider { get; set; }
        public ResponseHandler ResultHandler { get; set; }
    }

    public class Request
    {
        public GroupCollection Groups { get; set; }
        public int MessageId { get; set; }
    }

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

    public enum ResultType
    {
        Text,
        Photo,
        Document
    };

    public delegate Task ResponseHandler(Response response);
}
