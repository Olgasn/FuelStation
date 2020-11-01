using FuelStation.DataLayer.Data;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Controllers
{
    public class HomeController : Controller
    {
        private readonly FuelsContext _db;
        public HomeController(FuelsContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var fuels = _db.Fuels.Take(15).ToList();
            var tanks = _db.Tanks.Take(15).ToList();
            List<FilterOperationViewModel> operations = _db.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new FilterOperationViewModel { OperationID = t.OperationID, FuelType = t.Fuel.FuelType, TankType = t.Tank.TankType, Inc_Exp = t.Inc_Exp, Date = t.Date })
                .Take(15)
                .ToList();

            HomeViewModel homeViewModel = new HomeViewModel { Tanks = tanks, Fuels = fuels, Operations = operations };
            return View(homeViewModel);
        }



    }
}
