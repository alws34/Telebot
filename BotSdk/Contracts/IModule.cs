using BotSdk.Models;
using System.Threading.Tasks;

namespace BotSdk.Contracts
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

    public delegate Task ResponseHandler(Response response);
}
