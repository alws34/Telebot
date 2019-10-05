using System.Collections.Generic;

namespace Telebot.Commands.Status.Builder
{
    public class StatusBuilder
    {
        private readonly List<IStatus> _statuses;

        public StatusBuilder()
        {
            _statuses = new List<IStatus>();
        }

        public StatusBuilder Add(IStatus status)
        {
            _statuses.Add(status);
            return this;
        }

        public StatusBuilder AddRange(IStatus[] statuses)
        {
            _statuses.AddRange(statuses);
            return this;
        }

        public IStatus[] Build()
        {
            return _statuses.ToArray();
        }
    }
}
