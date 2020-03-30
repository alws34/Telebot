﻿using Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace PluginManager
{
    public class Plugins : IFactory<IPlugin>
    {
        [ImportMany]
        public IEnumerable<IPlugin> Items { get; set; }

        private readonly static Plugins instance = new Plugins();

        public static Plugins GetInstance()
        {
            return instance;
        }

        public Plugins()
        {
            var catalog = new AggregateCatalog();

            //Add all the parts found in the assembly located at this path
            var plugins = Directory.EnumerateFiles(".\\Plugins", "*Plugin.dll", SearchOption.AllDirectories);

            foreach (string plugin in plugins)
            {
                var assembly = Assembly.LoadFrom(plugin);
                var assemblyCatalog = new AssemblyCatalog(assembly);
                catalog.Catalogs.Add(assemblyCatalog);
            }

            //Create the CompositionContainer with the parts
            //in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(this);

            _items.AddRange(Items);
        }
    }
}
