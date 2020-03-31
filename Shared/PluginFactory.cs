using Common.Models;
using Contracts;
using Contracts.Factories;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Shared
{
    public class PluginFactory : IFactory<IPlugin>, IInitializer
    {
        [ImportMany(typeof(IPlugin))]
        private IEnumerable<IPlugin> Plugins { get; set; }

        public PluginFactory(ILoader loader)
        {
            loader.Load(this);

            _items.AddRange(Plugins);
        }

        public void Initialize(PluginData data)
        {
            foreach (IPlugin plugin in _items)
            {
                plugin.Initialize(data);
            }
        }
    }

    public interface IInitializer
    {
        void Initialize(PluginData data);
    }
}
