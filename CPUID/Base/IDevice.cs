using CPUID.Models;
using System.Collections.Generic;
using static CPUID.CpuIdWrapper64;

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

            Sdk64.GetSensorInfos(
                DeviceIndex, 0, sensorClass, ref sensorId,
                ref sensorName, ref rvalue, ref value, ref minVal, ref maxVal
             );

            return new Sensor(sensorName, value, minVal, maxVal);
        }

        public List<Sensor> GetSensors(int sensorClass)
        {
            int sensorCount = Sdk64.GetNumberOfSensors(DeviceIndex, sensorClass);

            var sensors = new List<Sensor>(sensorCount);

            for (int sensorIndex = 0; sensorIndex < sensorCount; sensorIndex++)
            {
                int sensorId = 0;
                string sensorName = "";
                int rvalue = 0;
                float value = 0.0f;
                float minVal = 0.0f;
                float maxVal = 0.0f;

                Sdk64.GetSensorInfos(
                    DeviceIndex, sensorIndex, sensorClass, ref sensorId,
                    ref sensorName, ref rvalue, ref value, ref minVal, ref maxVal
                 );

                Sensor sensor = new Sensor(sensorName, value, minVal, maxVal);

                sensors.Add(sensor);
            }

            return sensors;
        }

        public abstract override string ToString();
    }

    public abstract class IBattery : IDevice
    {

    }

    public abstract class IProcessor : IDevice
    {

    }

    public abstract class IDisplay : IDevice
    {

    }

    public abstract class IDrive : IDevice
    {

    }

    public abstract class IMainboard : IDevice
    {

    }
}
