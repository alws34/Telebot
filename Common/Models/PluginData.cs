using Contracts;
using Contracts.Factories;
using System;

namespace Common.Models
{
    public class PluginData
    {
        public IFactory<IPlugin> Plugins { get; set; }
        public Action Exit { get; set; }
        public Action Restart { get; set; }
    }
}
