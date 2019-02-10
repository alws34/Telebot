using System.IO;

namespace Telebot.Loggers
{
    public class FileLogger : ILogger
    {
        public void Log(string text)
        {
            File.WriteAllText("Exceptions.txt", text);
        }
    }
}
