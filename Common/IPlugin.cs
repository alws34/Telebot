using Common.Models;
using SimpleInjector;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class IPlugin
    {
        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        protected ResponseHandler respHandler;

        public abstract void Execute(Request req);

        public virtual void Initialize(ResponseHandler respHandler)
        {
            this.respHandler = respHandler;
        }

        public virtual void Initialize(Container iocContainer, ResponseHandler respHandler)
        {
            this.respHandler = respHandler;
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
