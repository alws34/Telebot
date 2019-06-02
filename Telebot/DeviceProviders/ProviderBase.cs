namespace Telebot.DeviceProviders
{
    public abstract class ProviderBase
    {
        protected float GetDeviceInfo(int sensorType, int deviceIndex)
        {
            int dummy = 0;

            return Program.pSDK.GetSensorTypeValue(sensorType, ref deviceIndex, ref dummy);
        }
    }
}
