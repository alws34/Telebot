﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Telebot.Common;
using Telebot.Jobs;

namespace Telebot.Intranet
{
    public abstract class IInetBase : IFeedback
    {
        protected const string wnetPath = ".\\wnet.exe";

        public JobType Jobtype { get; protected set; }

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
