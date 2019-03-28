using System.IO;

namespace Telebot.Loggers
{
    public class FileLogger : ILogger
    {
        public void Log(string text)
        {
            File.AppendAllText("Exceptions.txt", text);
        }
    }
}
