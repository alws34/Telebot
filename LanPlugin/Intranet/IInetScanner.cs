using System;
using System.Collections.Generic;

namespace LanPlugin.Intranet
{
    public abstract class IInetScanner : IInetBase
    {
        protected const string scanPath = ".\\wnetscan.xml";

        public event EventHandler<HostsArg> Discovered;

        protected void RaiseDiscovered(List<Host> hosts)
        {
            var arg = new HostsArg { Hosts = hosts };
            Discovered?.Invoke(this, arg);
        }

        public abstract void Discover();
    }
}
