using Common;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Telebot.Intranet
{
    public abstract class IInetBase : INotifyable
    {
        protected const string utilPath = ".\\WNetWatcher.exe";
        protected const string wcfgPath = ".\\wnet.cfg";

        protected List<Host> ReadHosts(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(HostsArg));

                var arg = (HostsArg)serializer.Deserialize(fileStream);

                return arg.Hosts;
            }
        }
    }
}
