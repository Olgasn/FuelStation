using Microsoft.AspNetCore.Mvc;
using FuelStation.ViewModels;
using FuelStation.Infrastructure.Filters;
using System;
using FuelStation.Data;
using System.Linq;
using System.Collections.Generic;

namespace FuelStation.Controllers
{
    public class CachedController : Controller
    {
        private FuelsContext _context;
        public CachedController(FuelsContext context)
        {
            _context = context;
        }
        [TypeFilter(typeof(CacheResourceFilterAttribute))]
        public IActionResult Index()
        {
            var fuels = _context.Fuels.Take(10).ToList();
            var tanks = _context.Tanks.Take(10).ToList();
            List<OperationViewModel> operations = _context.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new OperationViewModel
                {
                    OperationID = t.OperationID,
                    FuelType = t.Fuel.FuelType,
                    TankType = t.Tank.TankType,
                    Inc_Exp = t.Inc_Exp,
                    Date = t.Date
                })
                .Take(10)
                .ToList();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Tanks = tanks,
                Fuels = fuels,
                Operations = operations
            };
            return View("~/Views/Home/Index.cshtml", homeViewModel);
        }




    }
}
