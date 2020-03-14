using Common;
using CPUID.Base;
using CPUID.Factories;

namespace CPUID
{
    public static class CPUIDCore
    {
        public static CPUIDSDK Sdk { get; }

        public static IFactory<IDevice> DeviceFactory { get; }

        static CPUIDCore()
        {
            Sdk = new CPUIDSDK();
            Sdk.InitDLL();
            Sdk.InitSDK_Quick();

            DeviceFactory = new DeviceFactory();
        }
    }
}
