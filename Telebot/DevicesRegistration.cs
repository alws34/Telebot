using System;
using System.Collections.Generic;
using CPUID;
using CPUID.Base;
using CPUID.Devices;
using CPUID.Sdk;

namespace Telebot
{
    public class DevicesRegistration
    {
        private readonly int deviceCount;

        public DevicesRegistration()
        {
            deviceCount = CpuIdWrapper64.Sdk64.GetNumberOfDevices();
        }

        public IEnumerable<IDevice> GetDevices()
        {
            var cpuItems = LoadDevices<CPUDevice>(CpuIdSdk64.CLASS_DEVICE_PROCESSOR);
            var gpuItems = LoadDevices<GPUDevice>(CpuIdSdk64.CLASS_DEVICE_DISPLAY_ADAPTER);
            var ramItems = LoadDevices<RAMDevice>(CpuIdSdk64.CLASS_DEVICE_MAINBOARD);
            var hddItems = LoadDevices<HDDDevice>(CpuIdSdk64.CLASS_DEVICE_DRIVE);
            var batItems = LoadDevices<BATDevice>(CpuIdSdk64.CLASS_DEVICE_BATTERY);

            var result = new List<IDevice>();
            result.AddRange(cpuItems);
            result.AddRange(gpuItems);
            result.AddRange(ramItems);
            result.AddRange(hddItems);
            result.AddRange(batItems);

            return result;
        }

        private IEnumerable<T> LoadDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var items = new List<T>();

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (CpuIdWrapper64.Sdk64.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = CpuIdWrapper64.Sdk64.GetDeviceName(deviceIndex);

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