using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuelStation.Data;
using FuelStation.Models;

namespace FuelStation.Controllers
{
    public class TanksController : Controller
    {
        private readonly FuelsContext _context;

        public TanksController(FuelsContext context)
        {
            _context = context;
        }

        // GET: Tanks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tanks.ToListAsync());
        }

        // GET: Tanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Tanks
                .SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null)
            {
                return NotFound();
            }

            return View(tank);
        }

        // GET: Tanks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tanks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TankID,TankType,TankWeight,TankVolume,TankMaterial,TankPicture")] Tank tank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tank);
        }

        // GET: Tanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null)
            {
                return NotFound();
            }
            return View(tank);
        }

        // POST: Tanks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TankID,TankType,TankWeight,TankVolume,TankMaterial,TankPicture")] Tank tank)
        {
            if (id != tank.TankID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TankExists(tank.TankID))
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
            return View(tank);
        }

        // GET: Tanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Tanks
                .SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null)
            {
                return NotFound();
            }

            return View(tank);
        }

        // POST: Tanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);
            _context.Tanks.Remove(tank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TankExists(int id)
        {
            return _context.Tanks.Any(e => e.TankID == id);
        }
    }
}
