using Common.Contracts;
using SimpleInjector;

namespace Common.Models
{
    public class ModuleData
    {
        public Container IocContainer { get; set; }
        public ResponseHandler ResultHandler { get; set; }
    }
}
