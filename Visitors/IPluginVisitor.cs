using Plugins.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitors
{
    public abstract class IPluginVisitor
    {
        public abstract void Visit(HelpPlugin visit);
    }
}
