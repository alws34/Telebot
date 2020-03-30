using CPUID.Models;
using System.Collections.Generic;
using System.Text;

namespace Plugins.NSSpec.Components
{
    public abstract class IComponent
    {
        protected readonly StringBuilder stringResult;

        public IComponent()
        {
            stringResult = new StringBuilder();
        }

        protected void AppendSensors(string sensorName, IEnumerable<Sensor> sensors)
        {
            stringResult.AppendLine(sensorName);

            foreach (Sensor sensor in sensors)
            {
                string line = $"{sensor.Name} Value:{sensor.Value} Min:{sensor.Min} Max:{sensor.Max}";
                stringResult.AppendLine(line);
            }

            stringResult.AppendLine();
        }

        public abstract override string ToString();
    }
}
