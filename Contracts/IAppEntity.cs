using System;

namespace Contracts
{
    public class IAppEntity
    {
        public IFactory<IPlugin> Plugins { get; set; }
        public Action Exit { get; set; }
        public Action Restart { get; set; }
    }
}