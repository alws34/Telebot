using CPUID.Base;

namespace CPUID.Devices
{
    public class BATDevice : IDevice
    {
        public BATDevice()
        {

        }

        public BATDevice(string DeviceName, int DeviceIndex, uint DeviceClass)
        {
            this.DeviceName = DeviceName;
            this.DeviceIndex = DeviceIndex;
            this.DeviceClass = DeviceClass;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
