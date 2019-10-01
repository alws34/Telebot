using CPUID.Contracts;
using CPUID.Devices;
using System;
using System.Collections.Generic;

using static CPUID.CPUIDCore;

namespace CPUID.Factories
{
    public class DeviceFactory
    {
        public IDevice[] CPUDevices { get; }
        public IDevice[] GPUDevices { get; }
        public IDevice[] RAMDevices { get; }
        public IDevice[] HDDDevices { get; }
        public IDevice[] BATDevices { get; }

        private readonly int deviceCount;

        public DeviceFactory()
        {
            deviceCount = pSDK.GetNumberOfDevices();

            CPUDevices = GetDevices<CPUDevice>(CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            GPUDevices = GetDevices<GPUDevice>(CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            RAMDevices = GetDevices<RAMDevice>(CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            HDDDevices = GetDevices<HDDDevice>(CPUIDSDK.CLASS_DEVICE_DRIVE);
            BATDevices = GetDevices<BATDevice>(CPUIDSDK.CLASS_DEVICE_BATTERY);
        }

        private IDevice[] GetDevices<T>(uint deviceClass) where T : IDevice, new()
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
