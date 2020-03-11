using CPUID.Base;
using CPUID.Devices;
using System;
using System.Collections.Generic;

using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

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
            deviceCount = Sdk.GetNumberOfDevices();

            CPUDevices = GetDevices<CPUDevice>(CLASS_DEVICE_PROCESSOR);
            GPUDevices = GetDevices<GPUDevice>(CLASS_DEVICE_DISPLAY_ADAPTER);
            RAMDevices = GetDevices<RAMDevice>(CLASS_DEVICE_MAINBOARD);
            HDDDevices = GetDevices<HDDDevice>(CLASS_DEVICE_DRIVE);
            BATDevices = GetDevices<BATDevice>(CLASS_DEVICE_BATTERY);
        }

        private IDevice[] GetDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var devArr = new List<IDevice>();

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (Sdk.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = Sdk.GetDeviceName(deviceIndex);

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
