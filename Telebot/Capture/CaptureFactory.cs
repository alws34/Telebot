using Common;
using Telebot.Contracts;

namespace Telebot.Capture
{
    public class CaptureFactory : IFactory<IJob<CaptureArgs>>
    {
        public CaptureFactory()
        {
            _items.Add(new CaptureSchedule());
        }
    }
}
