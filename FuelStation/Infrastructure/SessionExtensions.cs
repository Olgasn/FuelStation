using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FuelStation.Infrastructure
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static void Set(this ISession session, string key, Dictionary<string, string> dictionary)
        {
            session.SetString(key, JsonConvert.SerializeObject(dictionary));
        }

        public static Dictionary<string, string> Get(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(Dictionary<string, string>) : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }

 
    }
}
