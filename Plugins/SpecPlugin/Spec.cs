using Contracts.Factories;
using CPUID.Base;
using Plugins.NSSpec.Components;
using Plugins.NSSpec.Sensors;
using Plugins.NSSpec.Sensors.Contracts;
using System.Text;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace Plugins.NSSpec
{
    public class Spec
    {
        private readonly IFactory<IDevice> deviceFactory;

        public Spec(IFactory<IDevice> deviceFactory)
        {
            this.deviceFactory = deviceFactory;
        }

        public string GetInfo()
        {
            var text = new StringBuilder();

            var processors = deviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_PROCESSOR);
            var storage = deviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_DRIVE);
            var displays = deviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER);
            var batteries = deviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_BATTERY);
            var mainboard = deviceFactory.FindAll(x => x.DeviceClass == CLASS_DEVICE_MAINBOARD);

            IComponent[] components = {
                new Processor(
                    processors,
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
                    storage,
                    new ISensor[]
                    {
                        new Temperature(),
                        new Utilization()
                    }
                ),
                new Display(
                    displays,
                    new ISensor[]
                    {
                        new Temperature()
                    }
                ),
                new Battery(
                    batteries,
                    new ISensor[]
                    {
                        new Voltage(),
                        new Capacity(),
                        new Level()
                    }
                ),
                new Mainboard(
                    mainboard,
                    new ISensor[]
                    {
                        new Utilization()
                    }
                )
            };

            foreach (IComponent component in components)
            {
                text.Append(component);
            }

            return text.ToString();
        }
    }
}
