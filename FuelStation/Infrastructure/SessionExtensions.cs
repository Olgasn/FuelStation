using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FuelStation.Infrastructure
{
    // Методы расширения для ISession для работы с произвольными объектами
    public static class SessionExtensions
    {
        //Запись произвольного объекта  в сессию
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        //Считывание произвольного объекта из сессии
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }


    }
}
