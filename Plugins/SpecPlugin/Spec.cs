using CPUID.Base;
using Plugins.NSSpec.Components;
using Plugins.NSSpec.Sensors;
using Plugins.NSSpec.Sensors.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CPUID.Sdk.CpuIdSdk64;

namespace Plugins.NSSpec
{
    public class Spec
    {
        private readonly IEnumerable<IDevice> processors;
        private readonly IEnumerable<IDevice> storage;
        private readonly IEnumerable<IDevice> displays;
        private readonly IEnumerable<IDevice> batteries;
        private readonly IEnumerable<IDevice> mainboard;

        public Spec(IEnumerable<IDevice> devices)
        {
            processors = devices.Where(x => x.DeviceClass == CLASS_DEVICE_PROCESSOR);
            storage = devices.Where(x => x.DeviceClass == CLASS_DEVICE_DRIVE);
            displays = devices.Where(x => x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER);
            batteries = devices.Where(x => x.DeviceClass == CLASS_DEVICE_BATTERY);
            mainboard = devices.Where(x => x.DeviceClass == CLASS_DEVICE_MAINBOARD);
        }

        public string GetInfo()
        {
            var text = new StringBuilder();

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
