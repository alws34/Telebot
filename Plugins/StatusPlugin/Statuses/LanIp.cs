using BotSdk.Contracts;
using System.Net;
using System.Net.Sockets;

namespace StatusPlugin.Statuses
{
    public class LanIp : IClassStatus
    {
        public string GetStatus()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return $"*LAN IPv4*: {address}";
                }
            }

            return "LanIp Error";
        }
    }
}
