namespace CPUID
{
    public static class CPUIDCore
    {
        public static CPUIDSDK pSDK { get; }

        static CPUIDCore()
        {
            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            pSDK.InitSDK_Quick();
        }
    }
}
