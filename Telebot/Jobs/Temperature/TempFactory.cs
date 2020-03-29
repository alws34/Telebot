using Common;
using Telebot.Jobs;
using static CPUID.CpuIdWrapper64;
using static CPUID.Sdk.CpuIdSdk64;

namespace Telebot.Temperature
{
    public class TempFactory : IFactory<IJob<TempArgs>>
    {
        public TempFactory()
        {
            var devices = DeviceFactory.FindAll(x =>
                (x.DeviceClass == CLASS_DEVICE_PROCESSOR) ||
                (x.DeviceClass == CLASS_DEVICE_DISPLAY_ADAPTER)
            );

            var tempSettings = Program.AppSettings.Temperature;

            _items.Add(new TempWarning(devices, tempSettings));
            _items.Add(new TempSchedule(devices));
        }
    }
}
