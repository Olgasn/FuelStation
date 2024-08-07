﻿using FuelStation.Data;
using FuelStation.Infrastructure.Filters;
using FuelStation.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Controllers
{

    public class CachedController(FuelsContext context) : Controller
    {
        private readonly FuelsContext _context = context;

        // Кэширование с использования фильтра ресурсов
        [TypeFilter(typeof(CacheResourceFilterAttribute))]
        public IActionResult Index()
        {
            int numberRows = 10;
            List<Fuel> fuels = [.. _context.Fuels.Take(numberRows)];
            List<Tank> tanks = [.. _context.Tanks.Take(numberRows)];
            List<OperationViewModel> operations = [.. _context.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new OperationViewModel
                {
                    OperationID = t.OperationID,
                    FuelType = t.Fuel.FuelType,
                    TankType = t.Tank.TankType,
                    Inc_Exp = t.Inc_Exp,
                    Date = t.Date
                })
                .Take(numberRows)];

            HomeViewModel homeViewModel = new()
            {
                Tanks = tanks,
                Fuels = fuels,
                Operations = operations
            };
            return View("~/Views/Home/Index.cshtml", homeViewModel);
        }




    }
}
