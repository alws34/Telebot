using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public class ScreenCapFactory : IFactory<IJob<ScreenCaptureArgs>>
    {
        private readonly List<IJob<ScreenCaptureArgs>> screenCaptures;

        public ScreenCapFactory()
        {
            screenCaptures = new List<IJob<ScreenCaptureArgs>>
            {
                { new ScreenCaptureSchedule() }
            };
        }

        public IJob<ScreenCaptureArgs> FindEntity(Predicate<IJob<ScreenCaptureArgs>> predicate)
        {
            return screenCaptures.SingleOrDefault(x => predicate(x));
        }

        public IJob<ScreenCaptureArgs>[] GetAllEntities()
        {
            return screenCaptures.ToArray();
        }
    }
}
