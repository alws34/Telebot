using Common.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class IPlugin
    {
        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        protected ResponseHandler resultHandler;

        public abstract void Execute(Request req);

        public virtual void Initialize(PluginData data)
        {
            this.resultHandler = data.ResultHandler;
        }

        public virtual bool GetJobActive() => false;

        public virtual string GetJobName() => "";

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }

    public delegate Task ResponseHandler(Response response);
}
