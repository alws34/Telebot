using SpecInfo.Components;
using SpecInfo.Sensors;
using SpecInfo.Sensors.Contracts;
using System;
using System.Text;
using static CPUID.CpuIdWrapper64;

namespace SpecInfo
{
    public class Spec : IDisposable
    {
        public Spec()
        {
            Sdk64.RefreshInformation();
        }

        public void Dispose()
        {
            Sdk64.UninitSDK();
        }

        public string GetInfo()
        {
            var builder = new StringBuilder();

            IComponent[] components = new IComponent[]
            {
                new Processor(
                    new ISensor[]
                    {
                        new Voltage(),
                        new Temperature(),
                        new Power(),
                        new Utilization(),
                        new ClockSpeed()
                    }
                ),
                new Storage(
                    new ISensor[]
                    {
                        new Temperature(),
                        new Utilization()
                    }
                ),
                new Display(
                    new ISensor[]
                    {
                        new Temperature()
                    }
                ),
                new Battery(
                    new ISensor[]
                    {
                        new Voltage(),
                        new Capacity(),
                        new Level()
                    }
                ),
                new Mainboard(
                    new ISensor[]
                    {
                        new Utilization()
                    }
                )
            };

            foreach (IComponent component in components)
            {
                builder.Append(component.ToString());
            }

            return builder.ToString();
        }
    }
}
