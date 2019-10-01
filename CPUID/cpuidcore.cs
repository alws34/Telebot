using CPUID.Factories;

namespace CPUID
{
    public static class CPUIDCore
    {
        public static CPUIDSDK pSDK { get; }

        public static DeviceFactory DeviceFactory { get; }

        static CPUIDCore()
        {
            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            pSDK.InitSDK_Quick();

            DeviceFactory = new DeviceFactory();
        }
    }
}
