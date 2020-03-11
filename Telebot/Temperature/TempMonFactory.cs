using Common;
using CPUID.Builder;
using Telebot.Contracts;
using static CPUID.CPUIDCore;

namespace Telebot.Temperature
{
    public class TempMonFactory : IFactory<IJob<TempChangedArgs>>
    {
        public TempMonFactory()
        {
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            _items.Add(new TempMonWarning(devices));
            _items.Add(new TempMonSchedule(devices));
        }
    }
}
