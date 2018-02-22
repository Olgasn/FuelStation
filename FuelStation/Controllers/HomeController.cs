using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using FuelStation.Models;
using FuelStation.ViewModels;
using FuelStation.Data;
using Microsoft.Extensions.Caching.Memory;

namespace FuelStation.Controllers
{
    public class HomeController : Controller
    {
        private FuelsContext _db;
        private IMemoryCache _cache;
        public HomeController(FuelsContext db, IMemoryCache memoryCache)
            {
            _db = db;
            _cache = memoryCache;
        }
        public IActionResult Index()
        {
            HomeViewModel cacheEntry = _cache.Get<HomeViewModel>("Operations 10");

            return View(cacheEntry);
        }

        public IActionResult About()
        { 
        
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
    }
}
