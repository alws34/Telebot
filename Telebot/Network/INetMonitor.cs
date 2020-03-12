using System;

namespace Telebot.Network
{
    public abstract class INetMonitor
    {
        public event EventHandler<HostsArg> Discovered;

        protected void RaiseDiscoveredHosts(HostsArg args)
        {
            Discovered?.Invoke(this, args);
        }

        public abstract void Discover();
        public abstract void Listen();
        public abstract void Disconnect();
    }
}