using CPUID.Models;
using System.Collections.Generic;
using static CPUID.CPUIDCore;

namespace CPUID.Base
{
    public abstract class IDevice
    {
        public string DeviceName { get; protected set; }
        public int DeviceIndex { get; protected set; }
        public uint DeviceClass { get; protected set; }

        public Sensor GetSensor(int sensorClass)
        {
            int sensorId = 0;
            string sensorName = "";
            int rvalue = 0;
            float value = 0.0f;
            float minVal = 0.0f;
            float maxVal = 0.0f;

            Sdk.GetSensorInfos(
                this.DeviceIndex, 0, sensorClass, ref sensorId,
                ref sensorName, ref rvalue, ref value, ref minVal, ref maxVal
             );

            return new Sensor(sensorName, value, minVal, maxVal);
        }
        public List<Sensor> GetSensors(int sensorClass)
        {
            int sensorCount = Sdk.GetNumberOfSensors(this.DeviceIndex, sensorClass);

            var result = new List<Sensor>(sensorCount);

            for (int sensorIndex = 0; sensorIndex < sensorCount; sensorIndex++)
            {
                int sensorId = 0;
                string sensorName = "";
                int rvalue = 0;
                float value = 0.0f;
                float minVal = 0.0f;
                float maxVal = 0.0f;

                Sdk.GetSensorInfos(
                    this.DeviceIndex, sensorIndex, sensorClass, ref sensorId,
                    ref sensorName, ref rvalue, ref value, ref minVal, ref maxVal
                 );

                var sensor = new Sensor(sensorName, value, minVal, maxVal);

                result.Add(sensor);
            }

            return result;
        }

        public abstract override string ToString();
    }
}
