using System.Net;
using System.Net.Sockets;

namespace StatusPlugin.Statuses
{
    public class LanAddrStatus : IStatus
    {
        public string GetStatus()
        {
            return $"*LAN IPv4*: {GetLocalIPv4()}";
        }

        private string GetLocalIPv4()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }

            return "Issue @ GetLocalIPv4()";
        }
    }
}
