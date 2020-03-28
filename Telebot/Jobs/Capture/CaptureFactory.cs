using Common;
using Telebot.Jobs;

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
