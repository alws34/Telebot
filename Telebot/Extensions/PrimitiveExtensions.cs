namespace Telebot.Extensions
{
    public static class PrimitiveExtensions
    {
        public static string AsReadable(this bool value)
        {
            return value ? "Active ✅" : "Inactive ❌";
        }
    }
}
