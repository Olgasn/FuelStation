using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.Infrastructure;
using FuelStation.Infrastructure.Filters;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FuelStation.Controllers
{
    //[ExceptionFilter] // Фильтр исключений
    public class FilteredOperationsController : Controller
    {
        private readonly FuelsContext _context;
        private FilterOperationViewModel _operation = new FilterOperationViewModel
        {
            FuelType = "",
            TankType = ""
        };

        public FilteredOperationsController(FuelsContext context)
        {
            _context = context;
        }

        // GET: Operations
        [SetToSession("SortState")] //Фильтр действий для сохранение в сессию состояния сортировки
        public IActionResult Index(SortState sortOrder)
        {
            // Считывание данных из сессии
            var sessionOperation = HttpContext.Session.Get("Operation");
            var sessionSortState = HttpContext.Session.Get("SortState");
            if (sessionOperation != null)
                _operation = Transformations.DictionaryToObject<FilterOperationViewModel>(sessionOperation);
            if ((sessionSortState != null))
                if ((sessionSortState.Count > 0) & (sortOrder == SortState.No)) sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState["sortOrder"]);

            // Сортировка и фильтрация данных
            IQueryable<Operation> fuelsContext = _context.Operations;
            fuelsContext = Sort_Search(fuelsContext, sortOrder, _operation.TankType ?? "", _operation.FuelType ?? "");

            // Формирование модели для передачи представлению

            OperationsViewModel operations = new OperationsViewModel
            {
                Operations = fuelsContext,
                SortViewModel = new SortViewModel(sortOrder),
                FilterOperationViewModel = _operation
            };
            return View(operations);
        }
        // Post: Operations
        [HttpPost]
        [SetToSession("Operation")] //Фильтр действий для сохранение в сессию параметров отбора
        public IActionResult Index(FilterOperationViewModel operation)
        {
            // Считывание данных из сессии
            var sessionSortState = HttpContext.Session.Get("SortState");
            var sortOrder = new SortState();
            if (sessionSortState.Count > 0)
                sortOrder = (SortState)Enum.Parse(typeof(SortState), sessionSortState["sortOrder"]);

            // Сортировка и фильтрация данных
            IQueryable<Operation> fuelsContext = _context.Operations;
            fuelsContext = Sort_Search(fuelsContext, sortOrder, operation.TankType ?? "", operation.FuelType ?? "");

            // Формирование модели для передачи представлению
            OperationsViewModel operations = new OperationsViewModel
            {
                Operations = fuelsContext,
                SortViewModel = new SortViewModel(sortOrder),
                FilterOperationViewModel = operation
            };

            return View(operations);
        }
        // Сортировка и фильтрация данных
        private IQueryable<Operation> Sort_Search(IQueryable<Operation> operations, SortState sortOrder, string searchTankType, string searchFuelType)
        {
            switch (sortOrder)
            {
                case SortState.FuelTypeAsc:
                    operations = operations.OrderBy(s => s.Fuel.FuelType);
                    break;
                case SortState.FuelTypeDesc:
                    operations = operations.OrderByDescending(s => s.Fuel.FuelType);
                    break;
                case SortState.TankTypeAsc:
                    operations = operations.OrderBy(s => s.Tank.TankType);
                    break;
                case SortState.TankTypeDesc:
                    operations = operations.OrderByDescending(s => s.Tank.TankType);
                    break;
            }
            operations = operations.Include(o => o.Fuel).Include(o => o.Tank)
                .Where(o => o.Tank.TankType.Contains(searchTankType ?? "")
                & o.Fuel.FuelType.Contains(searchFuelType ?? ""));



            return operations;
        }



    }
}
