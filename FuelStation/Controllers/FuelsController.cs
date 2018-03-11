using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuelStation.Data;
using FuelStation.Models;

namespace FuelStation.Controllers
{
    public class FuelsController : Controller
    {
        private readonly FuelsContext _context;

        public FuelsController(FuelsContext context)
        {
            _context = context;
        }

        // GET: Fuels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fuels.ToListAsync());
        }

    }
}
