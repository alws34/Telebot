namespace Telebot.DeviceProviders
{
    public abstract class ProviderBase
    {
        protected float GetDeviceInfo(int sensorType, int deviceIndex, int sensorIndex = 0)
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

            return value;
        }
    }
}
