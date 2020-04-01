namespace Common
{
    public interface IStatus
    {
        string GetStatus();
    }

    public interface IClassStatus : IStatus
    {

    }

    public interface IModuleStatus : IStatus
    {

    }
}