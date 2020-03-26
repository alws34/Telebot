using Common.Models;
using System;

namespace Common
{
    public abstract class IFeedback
    {
        public event EventHandler<FeedbackArgs> Feedback;

        protected void RaiseFeedback(string text)
        {
            var arg = new FeedbackArgs
            {
                Text = text
            };

            Feedback?.Invoke(this, arg);
        }
    }
}
