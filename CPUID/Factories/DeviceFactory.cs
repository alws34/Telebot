using CPUID.Contracts;
using CPUID.Devices;
using System;
using System.Collections.Generic;

using static CPUID.CPUIDCore;

namespace CPUID.Factories
{
    public static class DeviceFactory
    {
        public static IDevice[] CPUDevices { get; }
        public static IDevice[] GPUDevices { get; }
        public static IDevice[] RAMDevices { get; }
        public static IDevice[] HDDDevices { get; }
        public static IDevice[] BATDevices { get; }

        private static int deviceCount;

        static DeviceFactory()
        {
            deviceCount = pSDK.GetNumberOfDevices();

            CPUDevices = GetDevices<CPUDevice>(CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            GPUDevices = GetDevices<GPUDevice>(CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            RAMDevices = GetDevices<RAMDevice>(CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            HDDDevices = GetDevices<HDDDevice>(CPUIDSDK.CLASS_DEVICE_DRIVE);
            BATDevices = GetDevices<BATDevice>(CPUIDSDK.CLASS_DEVICE_BATTERY);
        }

        private static IDevice[] GetDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var devArr = new List<IDevice>();

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (pSDK.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = pSDK.GetDeviceName(deviceIndex);

                    T device = (T)Activator.CreateInstance(typeof(T), new object[]
                    {
                        deviceName,
                        deviceIndex,
                        deviceClass
                    });

                    devArr.Add(device);
                }
            }

            return devArr.ToArray();
        }
    }
}
