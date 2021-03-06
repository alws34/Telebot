﻿using System;

namespace BotSdk.Contracts
{
    public interface IAppExit
    {
        Action Exit { get; }
    }

    public class AppExit : IAppExit
    {
        public Action Exit { get; }

        public AppExit(Action Exit)
        {
            this.Exit = Exit;
        }
    }
}
