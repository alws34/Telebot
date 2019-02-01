using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public class GPUProvider : BaseProvider, ITemperatureProvider
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
