using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.Controllers
{
    public class FuelsController : Controller
    {
        private readonly int pageSize = 10;   // количество элементов на странице


        private readonly FuelsContext _context;

        public FuelsController(FuelsContext context, IConfiguration appConfig=null)
        {
            _context = context;
            if (appConfig != null)  {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Fuels
        public IActionResult Index(string FuelType="", SortState sortOrder=SortState.No, int page = 1)
        {
            // Сортировка и фильтрация данных
            IQueryable<Fuel> fuelsContext = _context.Fuels;
            fuelsContext = Sort_Search(fuelsContext, sortOrder, FuelType ?? "");

            // Разбиение на страницы
            var count = fuelsContext.Count();
            fuelsContext = fuelsContext.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            FuelsViewModel fuels = new()
            {
                Fuels = fuelsContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FuelType = FuelType
            };

            return View(fuels);
        }

        // GET: Fuels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuel = await _context.Fuels
                .SingleOrDefaultAsync(m => m.FuelID == id);
            if (fuel == null)
            {
                return NotFound();
            }

            return View(fuel);
        }

        // GET: Fuels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fuels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FuelID,FuelType,FuelDensity")] Fuel fuel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(fuel);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Fuels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuel = await _context.Fuels.SingleOrDefaultAsync(m => m.FuelID == id);
            if (fuel == null)
            {
                return NotFound();
            }
            return View(fuel);
        }

        // POST: Fuels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FuelID,FuelType,FuelDensity")] Fuel fuel)
        {
            if (id != fuel.FuelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fuel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuelExists(fuel.FuelID))
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
            return View(fuel);
        }

        // GET: Fuels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuel = await _context.Fuels
                .SingleOrDefaultAsync(m => m.FuelID == id);
            if (fuel == null)
            {
                return NotFound();
            }

            return View(fuel);
        }

        // POST: Fuels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fuel = await _context.Fuels.SingleOrDefaultAsync(m => m.FuelID == id);
            _context.Fuels.Remove(fuel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuelExists(int id)
        {
            return _context.Fuels.Any(e => e.FuelID == id);
        }
        private static IQueryable<Fuel> Sort_Search(IQueryable<Fuel> fuels, SortState sortOrder, string FuelType)
        {
            switch (sortOrder)
            {
                case SortState.FuelTypeAsc:
                    fuels = fuels.OrderBy(s => s.FuelType);
                    break;
                case SortState.FuelTypeDesc:
                    fuels = fuels.OrderByDescending(s => s.FuelType);
                    break;
                case SortState.FuelDensityAsc:
                    fuels = fuels.OrderBy(s => s.FuelDensity);
                    break;
                case SortState.FuelDensityDesc:
                    fuels = fuels.OrderByDescending(s => s.FuelDensity);
                    break;

            }
            fuels = fuels.Where(o => o.FuelType.Contains(FuelType ?? ""));
            return fuels;
        }
    }
}
