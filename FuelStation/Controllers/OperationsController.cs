using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.Infrastructure;
using FuelStation.Infrastructure.Filters;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.Controllers
{
    public class OperationsController : Controller
    {
        private readonly FuelsContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public OperationsController(FuelsContext context, IConfiguration appConfig=null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Operations
        [SetToSession("Operation")] //Фильтр действий для сохранение в сессию параметров отбора
        public IActionResult Index(FilterOperationViewModel operation, SortState sortOrder = SortState.No, int page = 1)
        {
            if (operation.FuelType == null & operation.TankType == null)
            {
                // Считывание данных из сессии
                if (HttpContext != null)
                {
                    var sessionOperation = HttpContext.Session.Get("Operation");
                    if (sessionOperation != null)
                        operation = Transformations.DictionaryToObject<FilterOperationViewModel>(sessionOperation);

                }                
            }

            // Сортировка и фильтрация данных
            IQueryable<Operation> fuelsContext = _context.Operations;
            fuelsContext = Sort_Search(fuelsContext, sortOrder, operation.TankType ?? "", operation.FuelType ?? "");

            // Разбиение на страницы
            var count = fuelsContext.Count();
            fuelsContext = fuelsContext.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            OperationsViewModel operations = new()
            {
                Operations = fuelsContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterOperationViewModel = operation
            };
            return View(operations);
        }


        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Fuel)
                .Include(o => o.Tank)
                .SingleOrDefaultAsync(m => m.OperationID == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // GET: Operations/Create
        public IActionResult Create()
        {
            var fuels = _context.Fuels;
            if (fuels != null) ViewData["FuelID"] = new SelectList(fuels, "FuelID", "FuelType");
            var tanks = _context.Tanks;
            if (tanks != null) ViewData["TankID"] = new SelectList(tanks, "TankID", "TankType");

            return View();
        }

        // POST: Operations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationID,FuelID,TankID,Inc_Exp,Date")] Operation operation)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(operation);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation =  await _context.Operations.SingleOrDefaultAsync(m => m.OperationID == id);
            if (operation == null)
            {
                return NotFound();
            }

            var fuels = _context.Fuels;
            if (fuels != null) ViewData["FuelID"] = new SelectList(fuels, "FuelID", "FuelType", operation.FuelID);
            var tanks = _context.Tanks;
            if (tanks != null) ViewData["TankID"] = new SelectList(tanks, "TankID", "TankType", operation.TankID);

            return View(operation);
        }

        // POST: Operations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperationID,FuelID,TankID,Inc_Exp,Date")] Operation operation)
        {
            if (id != operation.OperationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationExists(operation.OperationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var fuels = _context.Fuels;
            if (fuels != null) ViewData["FuelID"] = new SelectList(fuels, "FuelID", "FuelType", operation.FuelID);
            var tanks = _context.Tanks;
            if (tanks != null) ViewData["TankID"] = new SelectList(tanks, "TankID", "TankType", operation.TankID);

            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Fuel)
                .Include(o => o.Tank)
                .SingleOrDefaultAsync(m => m.OperationID == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operation = await _context.Operations.SingleOrDefaultAsync(m => m.OperationID == id);
            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.OperationID == id);
        }
        private static IQueryable<Operation> Sort_Search(IQueryable<Operation> operations, SortState sortOrder, string searchTankType, string searchFuelType)
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
