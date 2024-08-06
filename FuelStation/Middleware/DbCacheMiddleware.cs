using FuelStation.Services;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace FuelStation.Middleware
{
    //Компонент middleware для выполнения кэширования
    public class DbCacheMiddleware(RequestDelegate next, IMemoryCache memoryCache, string cacheKey = "Operations 10")
    {
        private readonly RequestDelegate _next = next;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly string _cacheKey = cacheKey;

        public Task Invoke(HttpContext httpContext, IOperationService operationService)
        {
            // пытаемся получить элемент из кэша
            if (!_memoryCache.TryGetValue(_cacheKey, out HomeViewModel homeViewModel))
            {
                // если в кэше не найден элемент, получаем его от сервиса
                homeViewModel = operationService.GetHomeViewModel();
                // и сохраняем в кэше
                _memoryCache.Set(_cacheKey, homeViewModel,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            }

            return _next(httpContext);
        }
    }

    // Метод расширения, используемый для добавления промежуточного программного обеспечения в конвейер HTTP-запроса.
    public static class DbCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseOperatinCache(this IApplicationBuilder builder, string cacheKey)
        {
            return builder.UseMiddleware<DbCacheMiddleware>(cacheKey);
        }
    }
}
