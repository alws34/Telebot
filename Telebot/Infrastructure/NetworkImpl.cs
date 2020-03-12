using System.Net;
using System.Net.Sockets;

namespace Telebot.Infrastructure
{
    public class NetworkImpl
    {
        public string LANIPv4 { get; }
        public string WANIPv4 { get; }

        public NetworkImpl()
        {
            LANIPv4 = GetLocalIPv4();
            WANIPv4 = GetPublicIPv4().TrimEnd();
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

        public string GetPublicIPv4()
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString("https://icanhazip.com");
            }
        }
    }
}
