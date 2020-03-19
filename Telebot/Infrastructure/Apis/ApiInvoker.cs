using System;

namespace Telebot.Infrastructure.Apis
{
    public class ApiInvoker
    {
        public static ApiInvoker Instance { get; } = new ApiInvoker();

        public void Invoke(IApi api)
        {
            api.Action();
        }

        public void Invoke<T>(IApi<T> api, Action<T> callback)
        {
            callback(api.Func());
        }
    }
}
