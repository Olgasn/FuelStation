using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace FuelStation.Infrastructure.Filters
{
    // Простой фильтр ресурсов, кэширующий ViewResult 
    public class CacheResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static readonly Dictionary<string, object> _cache
            = new Dictionary<string, object>(); // Объект словаря, в который будем кэшировать данные
        private string _cacheKey; // ключ

        // Выполняется до выполнения метода контроллера, но после привязки данных передаваемых в контроллер
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue;
                }
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (!String.IsNullOrEmpty(_cacheKey) &&
            !_cache.ContainsKey(_cacheKey))
            {
                var result = context.Result as ViewResult;
                if (result != null)
                {
                    _cache.Add(_cacheKey, result);
                }
            }

        }
    }
}
