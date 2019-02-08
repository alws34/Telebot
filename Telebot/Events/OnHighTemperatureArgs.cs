namespace Telebot.Events
{
    public class OnHighTemperatureArgs : IApplicationEvent
    {
        public string Message { get; private set; }

        public OnHighTemperatureArgs(string message)
        {
            Message = message;
        }
    }
}
