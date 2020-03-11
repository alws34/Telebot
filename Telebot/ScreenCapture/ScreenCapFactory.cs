using System;
using System.Collections.Generic;
using System.Linq;
using Telebot.Contracts;

namespace Telebot.ScreenCapture
{
    public class ScreenCapFactory : IFactory<IJob<ScreenCaptureArgs>>
    {
        private readonly List<IJob<ScreenCaptureArgs>> _jobs;

        public ScreenCapFactory()
        {
            _jobs = new List<IJob<ScreenCaptureArgs>>
            {
                { new ScreenCaptureSchedule() }
            };
        }

        public IJob<ScreenCaptureArgs> FindEntity(Predicate<IJob<ScreenCaptureArgs>> predicate)
        {
            return _jobs.Find(x => predicate(x));
        }

        public IJob<ScreenCaptureArgs>[] GetAllEntities()
        {
            return _jobs.ToArray();
        }

        public bool TryGetEntity(Predicate<IJob<ScreenCaptureArgs>> predicate, out IJob<ScreenCaptureArgs> entity)
        {
            entity = _jobs.Find(x => predicate(x));
            return entity != null;
        }
    }
}
