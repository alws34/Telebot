using Common.Models;
using Contracts;
using Contracts.Factories;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace Telebot.Plugins
{
    public class PluginFactory : IFactory<IPlugin>
    {
        [ImportMany(typeof(IPlugin))]
        private IEnumerable<IPlugin> Items { get; set; }

        public PluginFactory()
        {
            var catalog = new AggregateCatalog();

            var assemblies = Directory.EnumerateFiles(
                ".\\Plugins", "*Plugin.dll", SearchOption.AllDirectories
            );

            foreach (string assemblyName in assemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyName);
                var assemblyCatalog = new AssemblyCatalog(assembly);
                catalog.Catalogs.Add(assemblyCatalog);
            }

            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            _items.AddRange(Items);
        }

        public void InitializePlugins(PluginData data)
        {
            foreach (IPlugin plugin in _items)
            {
                plugin.Initialize(data);
            }
        }
    }
}
