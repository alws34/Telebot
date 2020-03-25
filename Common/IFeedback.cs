using Common.Models;
using System;

namespace Common
{
    public abstract class IFeedback
    {
        public event EventHandler<NotifyArg> Feedback;

        protected void RaiseFeedback(string fb)
        {
            var arg = new NotifyArg
            {
                Text = fb
            };

            Feedback?.Invoke(this, arg);
        }
    }
}
