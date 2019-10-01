﻿namespace Telebot.Settings
{
    public interface ISettings
    {
        string ReadString(string section, string key);
        void WriteString(string section, string key, string value);

        T ReadObject<T>(string section, string key);
        void WriteObject<T>(string section, string key, T value);

        void AddProfiles(params IProfile[] profiles);
        void CommitChanges();
        void WriteChanges();
    }
}
