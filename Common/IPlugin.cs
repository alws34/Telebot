using Common.Models;
using SimpleInjector;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class IPlugin
    {
        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Execute(Request req, Func<Response, Task> resp);

        public virtual void Initialize(Container iocContainer)
        {

        }

        public Version MinOsVersion { get; protected set; }

        public virtual bool GetJobActive() => false;

        public virtual string GetJobName() => "";

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
