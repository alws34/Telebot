using System;
using System.Collections.Generic;

namespace Telebot.DeviceProviders
{
    public static class ProvidersFactory
    {
        private static readonly IDeviceProvider[] cpuProviders;
        private static readonly IDeviceProvider[] gpuProviders;
        private static readonly IDeviceProvider[] drvProviders;
        private static readonly IDeviceProvider[] ramProviders;

        static ProvidersFactory()
        {
            cpuProviders = GetProviders<CPUProvider>(CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            gpuProviders = GetProviders<GPUProvider>(CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            drvProviders = GetProviders<RAMProvider>(CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            ramProviders = GetProviders<DriveProvider>(CPUIDSDK.CLASS_DEVICE_DRIVE);
        }

        private static IDeviceProvider[] GetProviders<T>(uint deviceClass) where T : IDeviceProvider, new()
        {
            var devArr = new List<IDeviceProvider>();

            for (int deviceIndex = 0; deviceIndex < Program.pSDK.GetNumberOfDevices(); deviceIndex++)
            {
                if (Program.pSDK.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = Program.pSDK.GetDeviceName(deviceIndex);

                    int sensorCount = Program.pSDK.GetNumberOfSensors
                    (
                        deviceIndex,
                        CPUIDSDK.SENSOR_CLASS_UTILIZATION
                    );

                    T device = (T)Activator.CreateInstance(typeof(T), new object[]
                    {
                        deviceName,
                        deviceIndex,
                        deviceClass,
                        sensorCount
                    });

                    devArr.Add(device);
                }
            }

            return devArr.ToArray();
        }

        public static IDeviceProvider[] GetCPUProviders()
        {
            return cpuProviders;
        }

        public static IDeviceProvider[] GetGPUProviders()
        {
            return gpuProviders;
        }

        public static IDeviceProvider[] GetRAMProviders()
        {
            return drvProviders;
        }

        public static IDeviceProvider[] GetDriveProviders()
        {
            return ramProviders;
        }
    }
}
