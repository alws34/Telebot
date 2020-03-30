namespace Contracts.Settings
{
    public interface IProfiler
    {
        void AddProfile(IProfile profile);
        void CommitChanges();
        void WriteChanges();
    }
}
