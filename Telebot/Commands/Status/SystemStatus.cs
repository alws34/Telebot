﻿using CPUID.Contracts;
using System.Collections.Generic;
using System.Text;

namespace Telebot.Commands.Status
{
    public class SystemStatus : IStatus
    {
        private readonly List<IDevice> devices;

        public SystemStatus(params IDevice[][] devicesArg)
        {
            devices = new List<IDevice>();

            foreach (IDevice[] devicesArr in devicesArg)
            {
                devices.AddRange(devicesArr);
            }
        }

        public string Execute()
        {
            var strBuilder = new StringBuilder();

            foreach (IDevice device in devices)
            {
                strBuilder.AppendLine(device.ToString());
            }

            return strBuilder.ToString().TrimEnd();
        }
    }
}