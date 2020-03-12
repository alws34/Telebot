﻿using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class WanAddrStatus : IStatus
    {
        private readonly NetworkApi networkApi;

        public WanAddrStatus()
        {
            networkApi = new NetworkApi();
        }

        public string GetStatus()
        {
            return $"*WAN IPv4*: {networkApi.WANIPv4}";
        }
    }
}
