using CPUID.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;
using static CPUID.CPUIDCore;

namespace Telebot.Temperature
{
    public class TempMonFactory : IFactory<IJob<TempChangedArgs>>
    {
        private readonly List<IJob<TempChangedArgs>> _jobs;

        public TempMonFactory()
        {
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            _jobs = new List<IJob<TempChangedArgs>>
            {
                { new TempMonWarning(devices) },
                { new TempMonSchedule(devices) }
            };
        }

        public IJob<TempChangedArgs> FindEntity(Predicate<IJob<TempChangedArgs>> predicate)
        {
            return _jobs.SingleOrDefault(x => predicate(x));
        }

        public IJob<TempChangedArgs>[] GetAllEntities()
        {
            return _jobs.ToArray();
        }
    }
}
