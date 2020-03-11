﻿using System.Net;
using System.Net.Sockets;

namespace Telebot.Infrastructure
{
    public class NetworkApi
    {
        public string LocalIPv4Address { get; }

        public NetworkApi()
        {
            LocalIPv4Address = GetLocalIPAddress();
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }

            return "No network adapters with an IPv4 address in the system!";
        }
    }
}
