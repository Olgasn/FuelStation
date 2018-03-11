using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using FuelStation.Data;
using Microsoft.Extensions.Caching.Memory;

namespace FuelStation.Controllers
{
    public class CashedHomeController : Controller
    {
        private FuelsContext _db;
        private IMemoryCache _cache;
        public CashedHomeController(FuelsContext db, IMemoryCache memoryCache)
            {
            _db = db;
            _cache = memoryCache;
        }
        public IActionResult Index()
        {
            HomeViewModel cacheEntry = _cache.Get<HomeViewModel>("Operations 10");

            return View("~/Views/Home/Index.cshtml", cacheEntry);
        }

        
    }
}
