using System;
using System.Collections.Generic;

namespace Telebot.Intranet
{
    public abstract class IINetMonitor : IInetBase
    {
        public event EventHandler<HostsArg> Connected;
        public event EventHandler<HostsArg> Disconnected;

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