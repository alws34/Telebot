using System.Collections.Generic;

namespace Telebot.DeviceProviders
{
    public abstract class ProviderBase : IDeviceProvider
    {
        public string DeviceName { get; protected set; }
        public int DeviceIndex { get; protected set; }
        public uint DeviceClass { get; protected set; }
        public int SensorsCount { get; protected set; }

        public abstract IEnumerable<SensorInfo> GetTemperatureSensors();
        public abstract IEnumerable<SensorInfo> GetUtilizationSensors();

        protected IEnumerable<SensorInfo> GetSensorsInfo(int sensorType, int deviceIndex)
        {
            for (int sensorIndex = 0; sensorIndex <= SensorsCount; sensorIndex++)
            {
                int sensor_id = 0;
                string sensor_name = "";
                int rvalue = 0;
                float value = 0.0f;
                float minVal = 0.0f;
                float maxVal = 0.0f;

                Program.pSDK.GetSensorInfos
                (
                    deviceIndex,
                    sensorIndex,
                    sensorType,
                    ref sensor_id,
                    ref sensor_name,
                    ref rvalue,
                    ref value,
                    ref minVal,
                    ref maxVal
                 );

                yield return new SensorInfo(sensor_name, value);
            }
        }

        public abstract override string ToString();
    }
}
