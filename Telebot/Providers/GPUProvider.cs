using System;
using System.Collections.Generic;
using Telebot.Contracts;

namespace Telebot.Providers
{
    public class GPUProvider : BaseProvider, ITemperatureProvider, IUtilizationProvider
    {
        public List<IHardwareInfo> GetTemperature()
        {
            return GetDeviceInfoBySensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
        }

        public List<IHardwareInfo> GetUtilization()
        {
            return GetDeviceInfoBySensor(CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
        }
    }
}
