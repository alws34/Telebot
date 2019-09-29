using CPUID.Devices;
using CPUID.Factories;
using CPUID.Models;
using System.Collections.Generic;
using System.Text;

namespace SpecInfo
{
    public class Spec
    {
        private StringBuilder result;

        public string GetInfo()
        {
            result = new StringBuilder();

            GetMainboardInfo();
            GetProcessorInfo();
            GetStorageInfo();
            GetDisplayAdapterInfo();
            GetBatteryInfo();

            return result.ToString();
        }

        private void GetMainboardInfo()
        {
            foreach (RAMDevice device in DeviceFactory.RAMDevices)
            {
                result.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }
        }

        private void GetProcessorInfo()
        {
            foreach (CPUDevice device in DeviceFactory.CPUDevices)
            {
                result.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_VOLTAGE);
                AppendSensors("Voltages:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_POWER);
                AppendSensors("Powers:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_CLOCK_SPEED);
                AppendSensors("Clocks:", sensors);
            }
        }

        private void GetStorageInfo()
        {
            foreach (HDDDevice device in DeviceFactory.HDDDevices)
            {
                result.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }
        }

        private void GetDisplayAdapterInfo()
        {
            foreach (GPUDevice device in DeviceFactory.GPUDevices)
            {
                result.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_TEMPERATURE);
                AppendSensors("Temperatures:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_CLOCK_SPEED);
                AppendSensors("Clocks:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_UTILIZATION);
                AppendSensors("Utilization:", sensors);
            }
        }

        private void GetBatteryInfo()
        {
            foreach (BATDevice device in DeviceFactory.BATDevices)
            {
                result.AppendLine($"+ {device.DeviceName}");

                var sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_VOLTAGE);
                AppendSensors("Voltages:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_CAPACITY);
                AppendSensors("Capacities:", sensors);

                sensors = device.GetSensors(CPUIDSDK.SENSOR_CLASS_LEVEL);
                AppendSensors("Levels:", sensors);
            }
        }

        private void AppendSensors(string sensorType, IEnumerable<Sensor> sensors)
        {
            result.AppendLine(sensorType);

            foreach (Sensor sensor in sensors)
            {
                result.AppendLine($"{sensor.Name} Value:{sensor.Value} Min:{sensor.Max} Max:{sensor.Max}");
            }

            result.AppendLine();
        }
    }
}
