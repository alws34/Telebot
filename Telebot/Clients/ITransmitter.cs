using System.Threading.Tasks;
using Telebot.Models;

namespace Telebot.Clients
{
    public interface ITransmitter
    {
        Task Transmit(CommandResult data);
    }
}
