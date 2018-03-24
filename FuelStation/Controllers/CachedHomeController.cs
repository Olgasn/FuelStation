using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace FuelStation.Controllers
{
    // Выборка кэшированых данных из IMemoryCache
    public class CachedHomeController : Controller
    {
        private IMemoryCache _cache;
        public CachedHomeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public IActionResult Index()
        {
            //считывание данных из кэша
            HomeViewModel cacheEntry = _cache.Get<HomeViewModel>("Operations 10");

            return View("~/Views/Home/Index.cshtml", cacheEntry);
        }




    }
}
