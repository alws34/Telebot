using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Commands.Status.Builder
{
    public class StatusBuilder
    {
        private readonly List<IStatus> _statuses;

        public StatusBuilder()
        {
            _statuses = new List<IStatus>();
        }

        public StatusBuilder AddItem(IStatus status)
        {
            _statuses.Add(status);
            return this;
        }

        public StatusBuilder AddItems(IStatus[] statuses)
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
