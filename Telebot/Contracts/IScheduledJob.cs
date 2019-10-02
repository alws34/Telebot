using System;

namespace Telebot.Contracts
{
    public interface IScheduledJob
    {
        void Start(TimeSpan duration, TimeSpan interval);
    }
}
