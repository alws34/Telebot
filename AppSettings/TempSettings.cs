using AppSettings.Contracts;
using System;

namespace AppSettings
{
    public class TempSettings
    {
        private readonly ISettings settings;

        public TempSettings(ISettings settings)
        {
            this.settings = settings;
        }

        public float GetCPULimit()
        {
            string limitStr = settings.ReadString("Temperature", "CPU_TEMPERATURE_WARNING");

            bool success = float.TryParse(limitStr, out float limit);

            return success ? limit : 65.0f;
        }

        public void SaveCPULimit(float limit)
        {
            string limitStr = Convert.ToString(limit);

            settings.WriteString("Temperature", "CPU_TEMPERATURE_WARNING", limitStr);
        }

        public float GetGPULimit()
        {
            string limitStr = settings.ReadString("Temperature", "GPU_TEMPERATURE_WARNING");

            bool success = float.TryParse(limitStr, out float limit);

            return success ? limit : 65.0f;
        }

        public void SaveGPULimit(float level)
        {
            string fStr = Convert.ToString(level);

            settings.WriteString("Temperature", "GPU_TEMPERATURE_WARNING", fStr);
        }

        public bool GetMonitoringState()
        {
            string state = settings.ReadString("Temperature", "Enabled");

            bool success = bool.TryParse(state, out bool bStatus);

            return success ? bStatus : success;
        }

        public void SaveMonitoringState(bool state)
        {
            string stateStr = Convert.ToString(state);

            settings.WriteString("Temperature", "Enabled", stateStr);
        }
    }
}
