using System;
using System.Collections.Generic;

namespace LanPlugin.Intranet
{
    public abstract class IINetMonitor : IInetBase
    {
        protected const string scanPath = ".\\Plugins\\Lan\\wnetmon.xml";

        public EventHandler<HostsArg> Connected;
        public EventHandler<HostsArg> Disconnected;

        public bool IsActive { get; set; }

        protected void RaiseConnected(List<Host> hosts)
        {
            var arg = new HostsArg { Hosts = hosts };
            Connected?.Invoke(this, arg);
        }

        protected void RaiseDisconnected(List<Host> hosts)
        {
            var arg = new HostsArg { Hosts = hosts };
            Disconnected?.Invoke(this, arg);
        }

        public abstract void Listen();
        public abstract void Disconnect();
    }
}