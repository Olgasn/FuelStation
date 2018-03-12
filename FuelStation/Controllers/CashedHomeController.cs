using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using FuelStation.Data;
using Microsoft.Extensions.Caching.Memory;

namespace FuelStation.Controllers
{
    public class CashedHomeController : Controller
    {
        private IMemoryCache _cache;
        public CashedHomeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public IActionResult Index()
        {
            HomeViewModel cacheEntry = _cache.Get<HomeViewModel>("Operations 10");

            return View("~/Views/Home/Index.cshtml", cacheEntry);
        }

        
    }
}
