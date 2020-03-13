using System;

namespace Common
{
    public abstract class INotifyable
    {
        public event EventHandler<NotifyArg> Notify;

        protected void RaiseNotify(string err)
        {
            var arg = new NotifyArg
            {
                Text = err
            };

            Notify?.Invoke(this, arg);
        }
    }
}
