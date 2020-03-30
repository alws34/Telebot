using Common.Models;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class IPlugin
    {
        public string Pattern { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Execute(Request req, Func<Response, Task> resp);

        public Version MinOSVersion { get; protected set; }

        public virtual bool GetJobActive() => false;

        public virtual string GetJobName() => "";

        public virtual void Initialize(PluginData data) { }

        public override string ToString()
        {
            return $"*{Pattern}* - {Description}";
        }
    }
}
