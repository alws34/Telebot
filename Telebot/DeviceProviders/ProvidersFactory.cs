using System;
using System.Collections.Generic;

namespace Telebot.DeviceProviders
{
    public class ProvidersFactory
    {
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
            return GetProviders<CPUProvider>(CPUIDSDK.CLASS_DEVICE_PROCESSOR);
        }

        public static IDeviceProvider[] GetGPUProviders()
        {
            return GetProviders<GPUProvider>(CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
        }

        public static IDeviceProvider[] GetRAMProviders()
        {
            return GetProviders<RAMProvider>(CPUIDSDK.CLASS_DEVICE_MAINBOARD);
        }

        public static IDeviceProvider[] GetDriveProviders()
        {
            return GetProviders<DriveProvider>(CPUIDSDK.CLASS_DEVICE_DRIVE);
        }
    }
}
