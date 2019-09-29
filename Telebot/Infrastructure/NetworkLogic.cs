﻿using System;
using System.Net;
using System.Net.Sockets;

namespace Telebot.Infrastructure
{
    public class NetworkLogic
    {
        public string LocalIPv4Address { get; }

        public NetworkLogic()
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

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
