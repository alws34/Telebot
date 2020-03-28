using Common;
using Telebot.Intranet;

namespace Telebot.Jobs.Intranet
{
    public class InetFactory : IFactory<IInetBase>
    {
        public InetFactory()
        {
            _items.Add(new LanMonitor());
            _items.Add(new LanScanner());
        }
    }
}
