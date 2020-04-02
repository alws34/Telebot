using BotSdk.Contracts;
using System;

namespace BotSdk.Models
{
    public class ModuleData
    {
        public IServiceProvider IoCProvider { get; set; }
        public ResponseHandler ResultHandler { get; set; }
    }
}
