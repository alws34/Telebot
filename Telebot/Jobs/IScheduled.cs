﻿namespace Telebot.Jobs
{
    public interface IScheduled
    {
        void Start(int duration_in_sec, int interval_in_sec);
    }
}