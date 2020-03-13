using System;

namespace Telebot.Contracts
{
    public interface IScheduled
    {
        void Start(TimeSpan duration, TimeSpan interval);
    }
}
