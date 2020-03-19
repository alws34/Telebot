using System.Net;

namespace Telebot.Commands.Status
{
    public class WanAddrStatus : IStatus
    {
        public string GetStatus()
        {
            return $"*WAN IPv4*: {GetPublicIPv4().TrimEnd()}";
        }

        private string GetPublicIPv4()
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString("https://icanhazip.com");
            }
        }
    }
}
