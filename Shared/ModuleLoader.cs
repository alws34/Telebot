﻿using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace Shared
{
    public class ModuleLoader : ILoader
    {
        private CompositionContainer container;

        public ModuleLoader()
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

            container = new CompositionContainer(catalog);
        }

        public void Load(object obj)
        {
            container.ComposeParts(obj);
        }
    }

    public interface ILoader
    {
        void Load(object obj);
    }
}