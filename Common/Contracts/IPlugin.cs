using Common.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class IPlugin
    {
        protected ResponseHandler ResultHandler;

        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Execute(Request req);

        public virtual void Initialize(PluginData data)
        {
            ResultHandler = data.ResultHandler;
        }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }

    public delegate Task ResponseHandler(Response response);
}
