using System;
using BotSdk.Models;

namespace BotSdk.Jobs
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
        }
    }
}
