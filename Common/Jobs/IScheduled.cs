namespace Contracts.Jobs
{
    public interface IScheduled
    {
        void Start(int DurationSec, int IntervalSec);
    }
}
