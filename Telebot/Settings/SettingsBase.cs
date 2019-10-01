using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Telebot.Settings
{
    public class SettingsBase : FileIniDataParser, ISettings
    {
        private const string iniPath = @".\settings.ini";

        private readonly IniData iniData;

        private readonly List<IProfile> profiles;

        public SettingsBase()
        {
            profiles = new List<IProfile>();

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

            return JsonConvert.DeserializeObject<T>(value);
        }

        public void WriteObject<T>(string section, string key, T value)
        {
            string valueStr = JsonConvert.SerializeObject(value);

            WriteString(section, key, valueStr);
        }

        public void AddProfiles(params IProfile[] profiles)
        {
            this.profiles.AddRange(profiles);
        }

        public void SaveProfilesChanges()
        {
            foreach (IProfile profile in profiles)
            {
                profile.SaveChanges();
            }
        }

        public void CommitSettings()
        {
            WriteFile(iniPath, iniData);
        }
    }
}
