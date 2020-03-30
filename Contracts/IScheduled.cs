namespace Contracts
{
    public interface IScheduled
    {
        void Start(int DurationSec, int IntervalSec);
    }
}
