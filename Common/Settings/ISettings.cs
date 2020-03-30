namespace Contracts.Settings
{
    public interface ISettings
    {
        string ReadString(string section, string key);
        void WriteString(string section, string key, string value);

        T ReadObject<T>(string section, string key);
        void WriteObject(string section, string key, object value);
    }
}
