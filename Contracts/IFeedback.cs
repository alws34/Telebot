using Models;

namespace Contracts
{
    public abstract class IFeedback
    {
        protected void RaiseFeedback(string text)
        {
            Feedback feedback = new Feedback
            {
                Text = text
            };

            MessageHub.MessageHub.Instance.Publish(feedback);
        }
    }
}
