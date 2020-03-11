using System;
using System.Collections.Generic;
using Telebot.Contracts;

namespace Telebot.Infrastructure
{
    public class ApiLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> services;

        public static IServiceLocator Instance { get; } = new ApiLocator();

        private ApiLocator()
        {
            services = new Dictionary<Type, object>();
        }

        public T GetService<T>()
        {
            var objType = typeof(T);

            if (services.ContainsKey(objType))
                return (T)services[objType];
            else
            {
                T obj = Activator.CreateInstance<T>();
                services.Add(objType, obj);
                return obj;
            }
        }
    }
}
