﻿using System;
using System.Collections.Generic;
using System.Timers;
using Telebot.Devices;

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

        protected readonly List<IDevice> deviceProviders;

        public event EventHandler<TemperatureChangedArgs> TemperatureChanged;

        protected void RaiseTemperatureChanged(TemperatureChangedArgs e)
        {
            TemperatureChanged?.Invoke(this, e);
        }

        protected TemperatureMonitorBase()
        {
            timer = new Timer();
            deviceProviders = new List<IDevice>();
        }

        public void Start()
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
