using Common;
using CPUID.Base;
using CPUID.Devices;
using System;
using System.Collections.Generic;
using static CPUID.CPUIDCore;
using static CPUID.CPUIDSDK;

namespace CPUID.Factories
{
    public class DeviceFactory : IFactory<IDevice>
    {
        private readonly int deviceCount;

        public DeviceFactory()
        {
            deviceCount = Sdk.GetNumberOfDevices();

            var CPUItems = GetDevices<CPUDevice>(CLASS_DEVICE_PROCESSOR);
            var GPUItems = GetDevices<GPUDevice>(CLASS_DEVICE_DISPLAY_ADAPTER);
            var RAMItems = GetDevices<RAMDevice>(CLASS_DEVICE_MAINBOARD);
            var HDDItems = GetDevices<HDDDevice>(CLASS_DEVICE_DRIVE);
            var BATItems = GetDevices<BATDevice>(CLASS_DEVICE_BATTERY);

            _items.AddRange(CPUItems);
            _items.AddRange(GPUItems);
            _items.AddRange(RAMItems);
            _items.AddRange(HDDItems);
            _items.AddRange(BATItems);
        }

        private IEnumerable<T> GetDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var items = new List<T>();

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

                    items.Add(device);
                }
            }

            return items;
        }
    }
}
