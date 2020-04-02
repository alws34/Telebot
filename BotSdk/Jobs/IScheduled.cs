namespace BotSdk.Jobs
{
    public interface IScheduled
    {
        void Start(int durationSec, int intervalSec);
    }
}
