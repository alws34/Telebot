using Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Telebot.NSPlugins
{
    public class Plugins : IFactory<IPlugin>
    {
        [ImportMany]
        public IEnumerable<IPlugin> Items { get; set; }

        public static Plugins Instance { get; private set; }

        // Early initialization "hack"
        public static void CreateInstance()
        {
            Instance = new Plugins();
        }

        private Plugins()
        {
            var catalog = new AggregateCatalog();

            //Add all the parts found in the assembly located at this path
            var assemblies = Directory.EnumerateFiles(
                ".\\Plugins", "*Plugin.dll", SearchOption.AllDirectories
            );

            foreach (string assemblyName in assemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyName);
                var assemblyCatalog = new AssemblyCatalog(assembly);
                catalog.Catalogs.Add(assemblyCatalog);
            }

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(this);

            _items.AddRange(Items);

            var entity = new IPluginData
            {
                Plugins = this,
                Exit = Application.Exit,
                Restart = Application.Restart
            };

            foreach (IPlugin item in Items)
            {
                item.Initialize(entity);
            }
        }
    }
}
