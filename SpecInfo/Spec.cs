using SpecInfo.Components;
using System.Text;

using static CPUID.CPUIDCore;

namespace SpecInfo
{
    public class Spec
    {
        public Spec()
        {
            Sdk.RefreshInformation();
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
