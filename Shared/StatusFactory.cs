using Common;
using Contracts.Factories;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Shared
{
    public class StatusFactory : IFactory<IStatus>
    {
        [ImportMany(typeof(IStatus))]
        private IEnumerable<IStatus> items { get; set; }

        public StatusFactory(ILoader loader)
        {
            loader.Load(this);

            _items.AddRange(items);
        }
    }
}
