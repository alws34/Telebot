using System;
using System.Diagnostics;
using Telebot.Contracts;
using Telebot.Controllers;
using Telebot.Extensions;

namespace Telebot.Commands.StatusCommands
{
    public class UptimeCmd : IStatusCommand
    {
        private readonly SystemController sysControlelr;

        public UptimeCmd()
        {
            sysControlelr = Program.container.GetInstance<SystemController>();
        }

        public string Execute()
        {
            return $"*Uptime*: {sysControlelr.GetUptime()}";
        }
    }
}
