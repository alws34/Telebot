using Common;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public class ScreenCapFactory : IFactory<IJob<ScreenCaptureArgs>>
    {
        public ScreenCapFactory()
        {
            _items.Add(new ScreenCaptureSchedule());
        }
    }
}
