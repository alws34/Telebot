﻿using CPUID.Contracts;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Telebot.Temperature
{
    public abstract class TemperatureMonitorBase : ITemperatureMonitor
    {
        protected readonly Timer timer;

        public bool IsActive
        {
            get { return timer.Enabled; }
            protected set { timer.Enabled = value; }
        }

        protected readonly List<IDevice> devices;

        public event EventHandler<TemperatureChangedArgs> TemperatureChanged;

        protected void RaiseTemperatureChanged(TemperatureChangedArgs e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        protected TemperatureMonitorBase()
        {
            timer = new Timer();
            devices = new List<IDevice>();
        }

        public virtual void Start()
        {
            timer.Start();
        }

        public virtual void Start(TimeSpan duration, TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
