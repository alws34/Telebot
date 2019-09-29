﻿using Telebot.Devices;
using Telebot.Infrastructure;

namespace Telebot.Commands.Status
{
    public class SystemStatus : IStatus
    {
        private readonly SystemLogic systemLogic;

        public SystemStatus(params IDevice[][] deviceProviders)
        {
            systemLogic = new SystemLogic
            (
                deviceProviders
            );
        }

        public string Execute()
        {
            return systemLogic.GetDevicesInfo();
        }
    }
}
