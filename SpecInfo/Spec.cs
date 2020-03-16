using SpecInfo.Components;
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
                new Processor(),
                new Storage(),
                new Display(),
                new Battery(),
                new Mainboard()
            };

            foreach (IComponent component in components)
            {
                builder.Append(component.ToString());
            }

            return builder.ToString();
        }
    }
}
