using System;
using System.Collections.Generic;
using System.Text;
using Telebot.Contracts;
using Telebot.Models;

namespace Telebot.HwProviders
{
    public class BaseProvider
    {
        protected List<HardwareInfo> GetCpuInfo(int sensorType)
        {
            int dummy = 0;

            var result = new List<HardwareInfo>();

            float val = Program.pSDK.GetSensorTypeValue(sensorType, ref dummy, ref dummy);

            result.Add(new HardwareInfo
            {
                DeviceClass = CPUIDSDK.CLASS_DEVICE_PROCESSOR,
                Value = val
            });

            return result;
        }
        protected List<HardwareInfo> GetDeviceInfoBySensor(int sensorClass, uint deviceClass)
        {
            var result = new List<HardwareInfo>();

            for (int device_index = 0; device_index < Program.NbDevices; device_index++)
            {
                string device_name = Program.pSDK.GetDeviceName(device_index);

                if (Program.pSDK.GetDeviceClass(device_index) == deviceClass)
                {
                    int NbSensors = Program.pSDK.GetNumberOfSensors(device_index, sensorClass);

                    for (int sensor_index = 0; sensor_index < NbSensors; sensor_index++)
                    {
                        string sensor_name = "";
                        float val = 0.0f, min = 0.0f, max = 0.0f;
                        int sensor_id = 0, iValue = 0;

                        bool sensor_info = Program.pSDK.GetSensorInfos(device_index, sensor_index, sensorClass,
                            ref sensor_id, ref sensor_name, ref iValue, ref val, ref min, ref max);

                        if ((sensor_info == true) && (Math.Round(val, 0) >= 0))
                        {
                            result.Add
                            (
                                new HardwareInfo
                                {
                                    DeviceName = device_name,
                                    DeviceClass = deviceClass,
                                    Value = val
                                }
                            );
                        }
                    }
                }
            }

            return result;
        }
    }
}
