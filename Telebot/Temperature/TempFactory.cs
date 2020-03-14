using Common;
using CPUID.Builder;
using Telebot.Contracts;
using static CPUID.CPUIDCore;

namespace Telebot.Temperature
{
    public class TempFactory : IFactory<IJob<TempArgs>>
    {
        public TempFactory()
        {
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            var tempSettings = Program.Settings.Temperature;

            _items.Add(new TempWarning(devices, tempSettings));
            _items.Add(new TempSchedule(devices));
        }
    }
}
