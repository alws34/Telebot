using System;
using System.Net;
using System.Net.Sockets;
using Telebot.Contracts;
using Telebot.Controllers;

namespace Telebot.Commands.StatusCommands
{
    public class IPCmd : IStatusCommand
    {
        private readonly NetworkController netController;

        public IPCmd()
        {
            netController = Program.container.GetInstance<NetworkController>();
        }

        public string Execute()
        {
            return $"*IP*: {netController.IP}";
        }
    }
}
