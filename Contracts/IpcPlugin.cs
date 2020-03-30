using System.Collections.Generic;

namespace Contracts
{
    public class IpcPlugin
    {
        public IEnumerable<IPlugin> Plugins { get; set; }
    }
}
