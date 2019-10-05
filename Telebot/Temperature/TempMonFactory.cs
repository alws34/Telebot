﻿using CPUID.Builder;
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
            var devices = new DeviceBuilder()
                .AddRange(DeviceFactory.CPUDevices)
                .AddRange(DeviceFactory.GPUDevices)
                .Build();

            tempMons = new List<ITempMon>
            {
                { new TempMonWarning(devices) },
                { new TempMonSchedule(devices) }
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
