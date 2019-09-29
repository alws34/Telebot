using CPUID.Contracts;
using CPUID.Models;
using System.Collections.Generic;
using static CPUID.CPUIDCore;

namespace CPUID.Base
{
    public abstract class DeviceBase : IDevice
    {
        public string DeviceName { get; protected set; }
        public int DeviceIndex { get; protected set; }
        public uint DeviceClass { get; protected set; }

        public List<Sensor> GetSensors(int SensorClass)
        {
            int sensorCount = pSDK.GetNumberOfSensors(this.DeviceIndex, SensorClass);

            var result = new List<Sensor>(sensorCount);

            for (int sensorIndex = 0; sensorIndex < sensorCount; sensorIndex++)
            {
                int sensorId = 0;
                string sensorName = "";
                int rvalue = 0;
                float value = 0.0f;
                float minVal = 0.0f;
                float maxVal = 0.0f;

                pSDK.GetSensorInfos
                (
                    this.DeviceIndex, sensorIndex, SensorClass,
                    ref sensorId, ref sensorName, ref rvalue,
                    ref value, ref minVal, ref maxVal
                 );

                var sensor = new Sensor(sensorName, value, minVal, maxVal);

                result.Add(sensor);
            }

            return result;
        }

        public abstract override string ToString();
    }
}
