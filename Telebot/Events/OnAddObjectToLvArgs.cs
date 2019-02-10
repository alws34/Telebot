using Telebot.Models;

namespace Telebot.Events
{
    public class OnAddObjectToLvArgs : IApplicationEvent
    {
        public OnAddObjectToLvArgs(LvItem item)
        {
            Item = item;
        }
        public LvItem Item { get; private set; }
    }
}
