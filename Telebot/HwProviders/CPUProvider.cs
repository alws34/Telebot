using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public class CPUProvider : BaseProvider, ITemperatureProvider
    {
        public IEnumerable<HardwareInfo> GetTemperature()
        {
            return GetCpuInfo(CPUIDSDK.SENSOR_TEMPERATURE_CPU_DTS);
        }

        public IEnumerable<HardwareInfo> GetUtilization()
        {
            return GetCpuInfo(CPUIDSDK.SENSOR_UTILIZATION_CPU);
        }
    }
}
