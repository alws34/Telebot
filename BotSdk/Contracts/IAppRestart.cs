﻿using System;

namespace BotSdk.Contracts
{
    public interface IAppRestart
    {
        Action Restart { get; }
    }

    public class AppRestart : IAppRestart
    {
        public Action Restart { get; }

        public AppRestart(Action Restart)
        {
            this.Restart = Restart;
        }
    }
}