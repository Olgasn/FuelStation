using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FuelStation.Infrastructure
{
    // Методы расширения для ISession для работы с произвольными объектами и объектами типа Dictionary<string, string>
    public static class SessionExtensions
    {
        //Запись приизвольного объекта  в сессию
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        //Считывание параметризованного объекта из сессии
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        //Запись объекта типа Dictionary<string, string> в сессию
        public static void Set(this ISession session, string key, Dictionary<string, string> dictionary)
        {
            session.SetString(key, JsonConvert.SerializeObject(dictionary));
        }
        //Считывание объекта типа Dictionary<string, string> из сессии
        public static Dictionary<string, string> Get(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(Dictionary<string, string>) : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }

 
    }
}
