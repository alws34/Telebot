using Contracts.Jobs;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LanPlugin.Intranet
{
    public abstract class IInetBase : IFeedback
    {
        protected const string wnetPath = ".\\Plugins\\Lan\\wnet.exe";

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
