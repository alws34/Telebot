﻿using CPUID.Sdk;
using System;

namespace CPUID
{
    public static class CpuIdWrapper64
    {
        public static CpuIdSdk64 Sdk64 { get; }

        static CpuIdWrapper64()
        {
            Sdk64 = new CpuIdSdk64(out bool success);

            if (!success)
                throw new Exception("CpuIdSdk64 could not be initialized.");
        }
    }
}
