using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FuelStation.Data;
using FuelStation.Models;

namespace FuelStation.Controllers
{
    public class OperationsController : Controller
    {
        private readonly FuelsContext _context;

        public OperationsController(FuelsContext context)
        {
            _context = context;
        }

        // GET: Operations
        public async Task<IActionResult> Index()
        {
            var fuelsContext = _context.Operations.Include(o => o.Fuel).Include(o => o.Tank);
            return View(await fuelsContext.ToListAsync());
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
            ViewData["FuelID"] = new SelectList(_context.Fuels, "FuelID", "FuelType");
            ViewData["TankID"] = new SelectList(_context.Tanks, "TankID", "TankType");
            return View();
        }

        // POST: Operations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationID,FuelID,TankID,Inc_Exp,Date")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FuelID"] = new SelectList(_context.Fuels, "FuelID", "FuelType", operation.FuelID);
            ViewData["TankID"] = new SelectList(_context.Tanks, "TankID", "TankType", operation.TankID);
            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations.SingleOrDefaultAsync(m => m.OperationID == id);
            if (operation == null)
            {
                return NotFound();
            }
            ViewData["FuelID"] = new SelectList(_context.Fuels, "FuelID", "FuelType", operation.FuelID);
            ViewData["TankID"] = new SelectList(_context.Tanks, "TankID", "TankType", operation.TankID);
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
            ViewData["FuelID"] = new SelectList(_context.Fuels, "FuelID", "FuelType", operation.FuelID);
            ViewData["TankID"] = new SelectList(_context.Tanks, "TankID", "TankType", operation.TankID);
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
    }
}
