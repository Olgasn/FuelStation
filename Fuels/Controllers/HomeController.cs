using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Fuels.Models;
using Fuels.ViewModels;
using Fuels.Data;

namespace Fuels.Controllers
{
    public class HomeController : Controller
    {
        private FuelsContext _db;
        public HomeController(FuelsContext db)
            {
            _db = db;
        }
        public IActionResult Index()
        {
            var fuels = _db.Fuels.Take(10).ToList();
            var tanks = _db.Tanks.Take(10).ToList();
            List<OperationViewModel> operations = _db.Operations
                .OrderByDescending(d=>d.Date)
                .Select(t => new OperationViewModel { OperationID = t.OperationID, FuelType = t.Fuel.FuelType, TankType = t.Tank.TankType, Inc_Exp = t.Inc_Exp, Date = t.Date })
                .Take(10)
                .ToList();

            HomeViewModel homeViewModel = new HomeViewModel { Tanks=tanks, Fuels=fuels, Operations= operations};
            return View(homeViewModel);
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
