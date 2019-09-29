using System;
using System.Collections.Generic;

namespace Telebot.Devices
{
    public static class DeviceFactory
    {
        public static IDevice[] CPUDevices { get; }
        public static IDevice[] GPUDevices { get; }
        public static IDevice[] RAMDevices { get; }
        public static IDevice[] DrvDevices { get; }

        static DeviceFactory()
        {
            CPUDevices = GetDevices<CPUDevice>(CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            GPUDevices = GetDevices<GPUDevice>(CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            RAMDevices = GetDevices<RAMDevice>(CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            DrvDevices = GetDevices<DriveDevice>(CPUIDSDK.CLASS_DEVICE_DRIVE);
        }

        private static IDevice[] GetDevices<T>(uint deviceClass) where T : IDevice, new()
        {
            var devArr = new List<IDevice>();

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
    }
}
