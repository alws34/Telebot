﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.DeviceProviders
{
    public class RAMProvider : ProviderBase, IDeviceProvider
    {
        public string DeviceName { get; private set; }
        public int DeviceIndex { get; private set; }
        public uint DeviceClass { get; private set; }

        public RAMProvider()
        {

        }

        public RAMProvider(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public float GetTemperature()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_TEMPERATURE_DRAM, this.DeviceIndex);
        }

        public float GetUtilization()
        {
            return base.GetDeviceInfo(CPUIDSDK.SENSOR_CLASS_UTILIZATION, this.DeviceIndex);
        }
    }
}
