using System;

namespace TempWarnPlugin.Settings
{
    public class TempSettings : IniFileHandler
    {
        public float GetCpuLimit()
        {
            string limitStr = ReadString("Temperature", "CPU_TEMPERATURE_WARNING");

            bool success = float.TryParse(limitStr, out float limit);

            return success ? limit : 65.0f;
        }

        public void SaveCpuLimit(float limit)
        {
            string limitStr = Convert.ToString(limit);

            WriteString("Temperature", "CPU_TEMPERATURE_WARNING", limitStr);
        }

        public float GetGpuLimit()
        {
            string limitStr = ReadString("Temperature", "GPU_TEMPERATURE_WARNING");

            bool success = float.TryParse(limitStr, out float limit);

            return success ? limit : 65.0f;
        }

        public void SaveGpuLimit(float level)
        {
            string fStr = Convert.ToString(level);

            WriteString("Temperature", "GPU_TEMPERATURE_WARNING", fStr);
        }

        public bool GetMonitoringState()
        {
            string state = ReadString("Temperature", "Enabled");

            bool success = bool.TryParse(state, out bool bStatus);

            return success ? bStatus : success;
        }

        public void SaveMonitoringState(bool state)
        {
            string stateStr = Convert.ToString(state);

            WriteString("Temperature", "Enabled", stateStr);
        }
    }
}
