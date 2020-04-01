using CPUID;
using CPUID.Base;
using CPUID.Devices;
using CPUID.Sdk;
using System;
using System.Collections.Generic;

namespace Telebot
{
    public class DeviceCreator
    {
        private readonly int deviceCount;

        public DeviceCreator()
        {
            deviceCount = CpuIdWrapper64.Sdk64.GetNumberOfDevices();
        }

        public IEnumerable<IProcessor> GetProcessors()
        {
            return LoadDevices<CPUDevice>(CpuIdSdk64.CLASS_DEVICE_PROCESSOR);
        }

        public IEnumerable<IDisplay> GetDisplays()
        {
            return LoadDevices<GPUDevice>(CpuIdSdk64.CLASS_DEVICE_DISPLAY_ADAPTER);
        }

        public IEnumerable<IBattery> GetBatteries()
        {
            return LoadDevices<BATDevice>(CpuIdSdk64.CLASS_DEVICE_BATTERY);
        }

        public IEnumerable<IDrive> GetDrives()
        {
            return LoadDevices<HDDDevice>(CpuIdSdk64.CLASS_DEVICE_DRIVE);
        }

        public IEnumerable<IMainboard> GetMainboards()
        {
            return LoadDevices<RAMDevice>(CpuIdSdk64.CLASS_DEVICE_MAINBOARD);
        }

        public IEnumerable<IDevice> GetAll()
        {
            var all = new List<IDevice>();

            all.AddRange(GetProcessors());
            all.AddRange(GetDisplays());
            all.AddRange(GetDrives());
            all.AddRange(GetMainboards());
            all.AddRange(GetBatteries());

            return all;
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