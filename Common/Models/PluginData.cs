using Contracts;
using SimpleInjector;

namespace Common.Models
{
    public class PluginData
    {
        public Container iocContainer { get; set; }
        public ResponseHandler ResultHandler { get; set; }
    }
}
