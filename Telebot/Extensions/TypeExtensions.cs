using System;

namespace Telebot.Extensions
{
    public static class TypeExtensions
    {
        public static EventHandler<T> GetHandler<T>(this Type type, object target)
        {
            string handlerName = $"{type.Name}Handler";

            var handler = Delegate.CreateDelegate(
                typeof(EventHandler<T>), target, handlerName
            );

            return handler as EventHandler<T>;
        }
    }
}
