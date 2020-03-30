using Models;
using System;

namespace Contracts
{
    public abstract class IFeedback
    {
        public EventHandler<Feedback> Feedback;

        protected void RaiseFeedback(string text)
        {
            Feedback feedback = new Feedback
            {
                Text = text
            };

            Feedback?.Invoke(this, feedback);

            //MessageHub.MessageHub.Instance.Publish(feedback);
        }
    }
}
