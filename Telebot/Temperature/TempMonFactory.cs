using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;
using static CPUID.CPUIDCore;

namespace Telebot.Temperature
{
    public class TempMonFactory : IFactory<ITempMon>
    {
        private readonly List<ITempMon> tempMons;

        public TempMonFactory()
        {
            tempMons = new List<ITempMon>
            {
                {
                    new TempMonWarning(
                        DeviceFactory.CPUDevices,
                        DeviceFactory.GPUDevices
                    )
                },
                {
                    new TempMonSchedule(
                        DeviceFactory.CPUDevices,
                        DeviceFactory.GPUDevices
                    )
                }
            };
        }

        public ITempMon FindEntity(Predicate<ITempMon> predicate)
        {
            return tempMons.SingleOrDefault(x => predicate(x));
        }

        public ITempMon[] GetAllEntities()
        {
            return tempMons.ToArray();
        }
    }
}
