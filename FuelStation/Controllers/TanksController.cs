using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.Controllers
{
    public class TanksController : Controller
    {
        private readonly FuelsContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly int pageSize = 10;

        public TanksController(FuelsContext context, IWebHostEnvironment hostingEnvironment= null, IConfiguration appConfig = null)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Tanks
        public IActionResult Index(string TankType = "", int page = 1)
        {
            IQueryable<Tank> fuelsContext = _context.Tanks.Where(t => t.TankType.Contains(TankType ?? ""));

            var count = fuelsContext.Count();
            fuelsContext = fuelsContext.Skip((page - 1) * pageSize).Take(pageSize);

            TanksViewModel fuels = new()
            {
                Tanks = fuelsContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                TankType = TankType
            };

            return View(fuels);
        }

        // GET: Tanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null) return NotFound();

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
        public async Task<IActionResult> Create([Bind("TankID,TankType,TankWeight,TankVolume,TankMaterial")] Tank tank)
        {
            if (ModelState.IsValid)
            {
                // Обработка загрузки изображения
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        // Генерация уникального имени файла
                        var uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        tank.TankPicture = uniqueFileName;
                    }
                }

                _context.Add(tank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tank);
        }

        // GET: Tanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null) return NotFound();

            return View(tank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tank tank)
        {

            if (id != tank.TankID) return NotFound();
            var existingTank = await _context.Tanks.AsNoTracking().FirstOrDefaultAsync(t => t.TankID == id);


            if (ModelState.IsValid)
            {
                try
                {

                    if (Request!=null )
                    {
                        var file = Request.Form.Files[0];
                        if (file.Length > 0)
                        {
                            // Удаляем старое изображение

                            if (!string.IsNullOrEmpty(existingTank.TankPicture))
                            {
                                var oldFilePath = Path.Combine(_hostingEnvironment.WebRootPath,
                                    "images", existingTank.TankPicture);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }

                            // Генерация уникального имени файла
                            var uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            tank.TankPicture = uniqueFileName;
                        }
                    }
                    else
                    {
                        // Сохраняем существующее изображение
                        existingTank = await _context.Tanks.AsNoTracking()
                            .FirstOrDefaultAsync(t => t.TankID == id);
                        tank.TankPicture = existingTank.TankPicture;
                    }

                    _context.Update(tank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TankExists(tank.TankID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tank);
        }

        // GET: Tanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);
            if (tank == null) return NotFound();

            return View(tank);
        }

        // POST: Tanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tank = await _context.Tanks.SingleOrDefaultAsync(m => m.TankID == id);

            // Удаляем изображение
            if (!string.IsNullOrEmpty(tank.TankPicture))
            {
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", tank.TankPicture);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

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