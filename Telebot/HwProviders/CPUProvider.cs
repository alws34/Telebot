using System.Collections.Generic;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public class CPUProvider : BaseProvider, ITemperatureProvider
    {
        public List<IHardwareInfo> GetTemperature()
        {
            return GetCpuInfo(CPUIDSDK.SENSOR_TEMPERATURE_CPU_DTS);
        }

        public List<IHardwareInfo> GetUtilization()
        {
            return GetCpuInfo(CPUIDSDK.SENSOR_UTILIZATION_CPU);
        }
    }
}
