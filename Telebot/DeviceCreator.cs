using CPUID.Base;
using CPUID.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace Telebot
{
    public class DeviceCreator
    {
        private readonly int deviceCount;
        private readonly List<IDevice> devices;

        public DeviceCreator()
        {
            deviceCount = Sdk64.GetNumberOfDevices();

            devices = new List<IDevice>();
            devices.AddRange(LoadDevices<CPUDevice>(CLASS_DEVICE_PROCESSOR));
            devices.AddRange(LoadDevices<GPUDevice>(CLASS_DEVICE_DISPLAY_ADAPTER));
            devices.AddRange(LoadDevices<RAMDevice>(CLASS_DEVICE_MAINBOARD));
            devices.AddRange(LoadDevices<HDDDevice>(CLASS_DEVICE_DRIVE));
            devices.AddRange(LoadDevices<BATDevice>(CLASS_DEVICE_BATTERY));
        }

        public IEnumerable<IProcessor> GetProcessors()
        {
            return devices.Where(d => d.DeviceClass == CLASS_DEVICE_PROCESSOR).Cast<IProcessor>();
        }

        public IEnumerable<IDisplay> GetDisplays()
        {
            return devices.Where(d => d.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER).Cast<IDisplay>();
        }

        public IEnumerable<IBattery> GetBatteries()
        {
            return devices.Where(d => d.DeviceClass == CLASS_DEVICE_BATTERY).Cast<IBattery>();
        }

        public IEnumerable<IDrive> GetDrives()
        {
            return devices.Where(d => d.DeviceClass == CLASS_DEVICE_DRIVE).Cast<IDrive>();
        }

        public IEnumerable<IMainboard> GetMainboards()
        {
            return devices.Where(d => d.DeviceClass == CLASS_DEVICE_MAINBOARD).Cast<IMainboard>();
        }

        public IEnumerable<IDevice> GetAll()
        {
            return devices;
        }

        private IEnumerable<T> LoadDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var items = new List<T>();

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (Sdk64.GetDeviceClass(deviceIndex) == deviceClass)
                {
                    string deviceName = Sdk64.GetDeviceName(deviceIndex);

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