using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace FuelStation.Controllers
{
    // Выборка кэшированых данных из IMemoryCache
    public class CachedHomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public CachedHomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //считывание данных из кэша
            HomeViewModel homeViewModel = _memoryCache.Get<HomeViewModel>("Operations 10");
            return View("~/Views/Home/Index.cshtml", homeViewModel);
        }




    }
}
