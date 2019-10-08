using System;
using Telebot.Common;

namespace Telebot.Contracts
{
    public interface IJob<T>
    {
        event EventHandler<T> Update;
        JobType JobType { get; }
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
