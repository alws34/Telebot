using System.Net;
using BotSdk.Contracts;

namespace StatusPlugin.Statuses
{
    public class WanIp : IClassStatus
    {
        public string GetStatus()
        {
            using (WebClient wc = new WebClient())
            {
                string ip = wc.DownloadString("https://icanhazip.com");
                return $"*WAN IPv4*: {ip.TrimEnd()}";
            }
        }
    }
}
