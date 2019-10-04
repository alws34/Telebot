using IniParser;
using IniParser.Model;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Telebot.Settings
{
    public class SettingsBase : FileIniDataParser, ISettings
    {
        private const string iniPath = @".\settings.ini";

        private readonly IniData iniData;

        private readonly List<IProfile> profiles;
        private readonly JavaScriptSerializer serializer;

        public SettingsBase()
        {
            profiles = new List<IProfile>();
            serializer = new JavaScriptSerializer();

            if (!File.Exists(iniPath))
            {
                File.Create(iniPath);
            }

            try
            {
                iniData = ReadFile(iniPath);
            }
            catch
            {

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

            return serializer.Deserialize<T>(value);
        }

        public void WriteObject<T>(string section, string key, T value)
        {
            string valueStr = serializer.Serialize(value);

            WriteString(section, key, valueStr);
        }

        public void AddProfile(IProfile profile)
        {
            this.profiles.Add(profile);
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
