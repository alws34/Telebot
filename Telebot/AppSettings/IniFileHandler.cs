using Contracts;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Telebot.AppSettings
{
    public class IniFileHandler : FileIniDataParser, ISettings
    {
        private const string iniPath = @".\settings.ini";

        private readonly IniData iniData;
        private readonly List<IProfile> profiles;

        public IniFileHandler()
        {
            profiles = new List<IProfile>();

            if (!File.Exists(iniPath))
            {
                File.Create(iniPath);
                iniData = new IniData();
            }
            else
            {
                iniData = ReadFile(iniPath);
            }
        }

        public string ReadString(string section, string key)
        {
            return iniData[section][key] ?? "";
        }

        public void WriteString(string section, string key, string value)
        {
            iniData[section][key] = value;
        }

        public T ReadObject<T>(string section, string key)
        {
            string value = ReadString(section, key);

            if (string.IsNullOrEmpty(value))
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        public void WriteObject(string section, string key, object value)
        {
            string valueStr = JsonConvert.SerializeObject(value);

            WriteString(section, key, valueStr);
        }

        public void AddProfile(IProfile profile)
        {
            profiles.Add(profile);
        }

        public void CommitChanges()
        {
            foreach (IProfile profile in profiles)
            {
                profile.SaveChanges();
            }
        }

        public void WriteChanges()
        {
            WriteFile(iniPath, iniData);
        }
    }
}
