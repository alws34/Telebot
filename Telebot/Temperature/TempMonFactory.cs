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
            var builder = new DeviceBuilder()
                .AddItems(DeviceFactory.CPUDevices)
                .AddItems(DeviceFactory.GPUDevices)
                .Build();

            tempMons = new List<ITempMon>
            {
                { new TempMonWarning(builder) },
                { new TempMonSchedule(builder) }
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
