using System;
using System.Diagnostics;
using System.Text;
using Telebot.Extensions;

namespace Telebot.BusinessLogic
{
    public class SystemLogic
    {
        public string GetSystemStatus()
        {
            StringBuilder result = new StringBuilder();

            for (int device_index = 0; device_index < Program.NbDevices; device_index++)
            {
                int deviceClass = Program.pSDK.GetDeviceClass(device_index);

                switch (deviceClass)
                {
                    case (int)CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                    case (int)CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                    case (int)CPUIDSDK.CLASS_DEVICE_MAINBOARD:
                    case (int)CPUIDSDK.CLASS_DEVICE_DRIVE:
                        int NbUtilSensors = Program.pSDK.GetNumberOfSensors(device_index, CPUIDSDK.SENSOR_CLASS_UTILIZATION);
                        for (int util_idx = 0; util_idx < NbUtilSensors; util_idx++)
                        {
                            string sensor_name = "";
                            float val = 0.0f, min = 0.0f, max = 0.0f;
                            int sensor_id = 0, iValue = 0;

                            bool utilInfo = Program.pSDK.GetSensorInfos(device_index, util_idx, CPUIDSDK.SENSOR_CLASS_UTILIZATION,
                                ref sensor_id, ref sensor_name, ref iValue, ref val, ref min, ref max);

                            if ((utilInfo == true) && (Math.Round(val, 0) >= 0))
                            {
                                string s = "";

                                string valStr = Convert.ToString(Math.Round(val, 0));

                                switch (deviceClass)
                                {
                                    case (int)CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                                        s = $"*CPU Usage*: {valStr}%";
                                        break;
                                    case (int)CPUIDSDK.CLASS_DEVICE_MAINBOARD:
                                        s = $"*RAM Used.*: {valStr}%";
                                        break;
                                    case (int)CPUIDSDK.CLASS_DEVICE_DRIVE:
                                        s = $"*{sensor_name.ToUpper().Replace("SPACE", "Storage Used")}*: {valStr}%";
                                        break;
                                }

                                if (!string.IsNullOrEmpty(s))
                                    result.AppendLine(s);
                            }

                            if (sensor_name.Equals("Processor")) break;
                        }

                        if ((deviceClass == CPUIDSDK.CLASS_DEVICE_PROCESSOR) || deviceClass == CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER)
                        {
                            int NbTempSensors = Program.pSDK.GetNumberOfSensors(device_index, CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                            for (int temp_idx = 0; temp_idx < NbTempSensors; temp_idx++)
                            {
                                string sensor_name = "";
                                float val = 0.0f, min = 0.0f, max = 0.0f;
                                int sensor_id = 0, iValue = 0;

                                bool tempInfo = Program.pSDK.GetSensorInfos(device_index, temp_idx, CPUIDSDK.SENSOR_CLASS_TEMPERATURE,
                                    ref sensor_id, ref sensor_name, ref iValue, ref val, ref min, ref max);

                                if ((tempInfo == true) && (Math.Round(val, 0) >= 0))
                                {
                                    string s = "";

                                    switch (deviceClass)
                                    {
                                        case (int)CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER:
                                            string device_name = Program.pSDK.GetDeviceName(device_index);
                                            device_name = device_name.Substring(0, device_name.IndexOf(" "));
                                            s = $"*GPU {device_name}*: {Convert.ToString(Math.Round(val, 0))}°C";
                                            break;
                                        case (int)CPUIDSDK.CLASS_DEVICE_PROCESSOR:
                                            s = $"*CPU Temp*: {Convert.ToString(Math.Round(val, 0))}°C";
                                            break;
                                    }

                                    result.AppendLine(s);
                                }

                                if (sensor_name.Equals("Package")) break;
                            }
                        }
                        break;
                }
            }

            return result.ToString().TrimEnd();
        }

        public string GetUptime()
        {
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue();
                return TimeSpan.FromSeconds(uptime.NextValue()).ToReadable();
            }
        }
    }
}
