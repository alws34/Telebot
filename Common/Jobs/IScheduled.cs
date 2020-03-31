namespace Contracts.Jobs
{
    public interface IScheduled
    {
        void Start(int durationSec, int intervalSec);
    }
}
