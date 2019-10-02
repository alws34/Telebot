namespace Telebot.Contracts
{
    public interface IServiceLocator
    {
        T GetService<T>();
    }
}
