using Common;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Telebot.Intranet
{
    public abstract class IInetBase : INotifyable
    {
        protected const string utilPath = ".\\WNetWatcher.exe";
        protected const string scanPath = ".\\scan.xml";

        protected List<Host> GetHosts()
        {
            using (var fileStream = new FileStream(scanPath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(HostsArg));

                var arg = (HostsArg)serializer.Deserialize(fileStream);

                return arg.Hosts;
            }
        }
    }
}
