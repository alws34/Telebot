using CPUID.Base;
using System;

namespace CPUID.Devices
{
    public class BATDevice : DeviceBase
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
            throw new NotImplementedException();
        }
    }
}
