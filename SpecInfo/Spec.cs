using System;
using System.Text;

namespace SpecInfo
{
    public class Spec
    {
        private readonly CPUIDSDK pSDK;

        public Spec()
        {
            pSDK = new CPUIDSDK();
            pSDK.InitDLL();
            pSDK.InitSDK();
            pSDK.RefreshInformation();
        }

        ~Spec()
        {
            pSDK.UninitSDK();
        }

        public string GetInfo()
        {
            var result = new StringBuilder();

            GetMainboardInfo(ref result);
            GetProcessorInfo(ref result);
            GetStorageInfo(ref result);
            GetDisplayAdapterInfo(ref result);
            GetBatteryInfo(ref result);

            return result.ToString();
        }

        private void GetMainboardInfo(ref StringBuilder result)
        {
            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_MAINBOARD);
            result.AppendLine();
        }

        private void GetProcessorInfo(ref StringBuilder result)
        {
            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_VOLTAGE, CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_POWER, CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_CLOCK_SPEED, CPUIDSDK.CLASS_DEVICE_PROCESSOR);
            result.AppendLine();
        }

        private void GetStorageInfo(ref StringBuilder result)
        {
            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_DRIVE);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_DRIVE);
            result.AppendLine();
        }

        private void GetDisplayAdapterInfo(ref StringBuilder result)
        {
            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_TEMPERATURE, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_CLOCK_SPEED, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_UTILIZATION, CPUIDSDK.CLASS_DEVICE_DISPLAY_ADAPTER);
            result.AppendLine();
        }

        private void GetBatteryInfo(ref StringBuilder result)
        {
            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_VOLTAGE, CPUIDSDK.CLASS_DEVICE_BATTERY);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_CAPACITY, CPUIDSDK.CLASS_DEVICE_BATTERY);
            result.AppendLine();

            GetValues(ref result, CPUIDSDK.SENSOR_CLASS_LEVEL, CPUIDSDK.CLASS_DEVICE_BATTERY);
        }

        private void GetValues(ref StringBuilder result, int sensor_class, uint device_class)
        {
            for (int device_index = 0; device_index < pSDK.GetNumberOfDevices(); device_index++)
            {
                if (pSDK.GetDeviceClass(device_index) == device_class)
                {
                    string device_name = pSDK.GetDeviceName(device_index);

                    if (!result.ToString().Contains(device_name))
                    {
                        result.AppendLine($"{device_name}:");
                    }

                    switch (sensor_class)
                    {
                        case CPUIDSDK.SENSOR_CLASS_TEMPERATURE:
                            result.AppendLine("Temperatures:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_UTILIZATION:
                            result.AppendLine("Utilization:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_VOLTAGE:
                            result.AppendLine("Voltages:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_POWER:
                            result.AppendLine("Powers:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_CLOCK_SPEED:
                            result.AppendLine("Clocks:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_CAPACITY:
                            result.AppendLine("Capacities:");
                            break;
                        case CPUIDSDK.SENSOR_CLASS_LEVEL:
                            result.AppendLine("Levels:");
                            break;
                    }

                    int NbSensors = pSDK.GetNumberOfSensors(device_index, sensor_class);

                    for (int sensor_index = 0; sensor_index < NbSensors; sensor_index++)
                    {
                        string sensor_name = "";
                        float val = 0.0f, min = 0.0f, max = 0.0f;
                        int sensor_id = 0, iValue = 0;

                        bool sensor_info = pSDK.GetSensorInfos(device_index, sensor_index, sensor_class,
                            ref sensor_id, ref sensor_name, ref iValue, ref val, ref min, ref max);

                        if ((sensor_info == true) && (Math.Round(val, 0) >= 0))
                        {
                            result.AppendLine($"{sensor_name} Value:{val} Min:{min} Max:{max}");
                        }
                    }
                }
            }
        }
    }
}
