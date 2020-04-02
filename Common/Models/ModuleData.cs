using System;
using System.Collections.Generic;
using Common.Contracts;
using SimpleInjector;

namespace Common.Models
{
    public class ModuleData
    {
        public IServiceProvider IoCProvider { get; set; }
        public ResponseHandler ResultHandler { get; set; }
    }
}
