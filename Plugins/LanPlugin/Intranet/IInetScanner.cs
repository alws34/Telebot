using System;
using System.Collections.Generic;

namespace LanPlugin.Intranet
{
    public abstract class IInetScanner : IInetBase
    {
        protected const string scanPath = ".\\Plugins\\Lan\\wnetscan.xml";

        public EventHandler<HostsArg> Discovered;

        protected void RaiseDiscovered(List<Host> hosts)
        {
            var arg = new HostsArg { Hosts = hosts, State = "Discovered" };
            Discovered?.Invoke(this, arg);
        }

        public abstract void Discover();
    }
}
