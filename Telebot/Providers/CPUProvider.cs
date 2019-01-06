using System.Collections.Generic;
using Telebot.Contracts;

namespace Telebot.Providers
{
    public class CPUProvider : BaseProvider, ITemperatureProvider, IUtilizationProvider
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
