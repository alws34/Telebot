using System;

namespace Telebot.Settings
{
    public class MonitorSettings : SettingsBase
    {
        private readonly ISettings settings;

        public MonitorSettings(ISettings settings)
        {
            this.settings = settings;
        }

        public float GetCPUWarningLevel()
        {
            string value = settings.ReadString("Temperature.Monitor", "CPU_TEMPERATURE_WARNING");

            float fValue = 65.0f;

            float.TryParse(value, out fValue);

            return fValue;
        }

        public void SaveCPUWarningLevel(float level)
        {
            string fStr = Convert.ToString(level);

            settings.WriteString("Temperature.Monitor", "CPU_TEMPERATURE_WARNING", fStr);
        }

        public float GetGPUWarningLevel()
        {
            string value = settings.ReadString("Temperature.Monitor", "GPU_TEMPERATURE_WARNING");

            float fValue = 65.0f;

            float.TryParse(value, out fValue);

            return fValue;
        }

        public void SaveGPUWarningLevel(float level)
        {
            string fStr = Convert.ToString(level);

            settings.WriteString("Temperature.Monitor", "GPU_TEMPERATURE_WARNING", fStr);
        }

        public bool GetTempMonitorStatus()
        {
            string status = settings.ReadString("Temperature.Monitor", "Enabled");

            bool bStatus = false;

            bool.TryParse(status, out bStatus);

            return bStatus;
        }

        public void SaveTempMonitorStatus(bool status)
        {
            string statusStr = Convert.ToString(status);

            settings.WriteString("Temperature.Monitor", "Enabled", statusStr);
        }
    }
}
