using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public class GPUProvider : BaseProvider, ITemperatureProvider
    {
        public IEnumerable<HardwareInfo> GetTemperature()
        {
            return GetDeviceInfoBySensor(CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
        }

        public IEnumerable<HardwareInfo> GetUtilization()
        {
            return GetDeviceInfoBySensor(CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
        }
    }
}
