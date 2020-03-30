using Contracts;
using ORMi;
using System.Linq;

namespace DimPlugin.Core
{
    public class DimApi : IApi
    {
        private readonly int level;

        public DimApi(int level)
        {
            this.level = level;

            Action = SetBrightness;
        }

        public void SetBrightness()
        {
            WMIHelper helper = new WMIHelper("Root\\wmi");
            var instance = helper.Query<BrightnessMethods>();
            instance.ElementAt(0).WmiSetBrightness(0, level);
        }
    }

    [WMIClass(Name = "WmiMonitorBrightnessMethods")]
    public class BrightnessMethods : WMIInstance
    {
        [WMIProperty("Active")]
        public bool Active { get; set; }

        [WMIProperty("InstanceName")]
        public string Name { get; set; }

        public int WmiSetBrightness(int timeOut, int brightness)
        {
            return WMIMethod.ExecuteMethod<int>(this, new { Timeout = timeOut, Brightness = brightness });
        }
    }
}
