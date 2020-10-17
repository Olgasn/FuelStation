using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using FuelStation.Data;
using FuelStation.Infrastructure.Filters;

namespace FuelStation.Controllers
{
    [ExceptionFilter]
    [TypeFilter(typeof(TimingLogAttribute))]
    public class HomeController : Controller
    {
        private FuelsContext _db;
        public HomeController(FuelsContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            int numberRows = 10;
            var fuels = _db.Fuels.Take(numberRows).ToList();
            var tanks = _db.Tanks.Take(numberRows).ToList();
            List<OperationViewModel> operations = _db.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new OperationViewModel { OperationID = t.OperationID, FuelType = t.Fuel.FuelType, TankType = t.Tank.TankType, Inc_Exp = t.Inc_Exp, Date = t.Date })
                .Take(numberRows)
                .ToList();

            HomeViewModel homeViewModel = new HomeViewModel { Tanks = tanks, Fuels = fuels, Operations = operations };
            return View(homeViewModel);
        }



    }
}
