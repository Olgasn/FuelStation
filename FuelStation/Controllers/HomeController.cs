using FuelStation.Data;
using FuelStation.Infrastructure.Filters;
using FuelStation.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Controllers
{
    [ExceptionFilter]
    [TypeFilter(typeof(TimingLogAttribute))]
    public class HomeController(FuelsContext db) : Controller
    {
        private readonly FuelsContext _db = db;

        public IActionResult Index()
        {
            int numberRows = 10;
            List<Fuel> fuels = [.. _db.Fuels.Take(numberRows)];
            List<Tank> tanks = [.. _db.Tanks.Take(numberRows)];
            List<OperationViewModel> operations = [.. _db.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new OperationViewModel { OperationID = t.OperationID, FuelType = t.Fuel.FuelType, TankType = t.Tank.TankType, Inc_Exp = t.Inc_Exp, Date = t.Date })
                .Take(numberRows)];

            HomeViewModel homeViewModel = new() { Tanks = tanks, Fuels = fuels, Operations = operations };
            return View(homeViewModel);
        }



    }
}
