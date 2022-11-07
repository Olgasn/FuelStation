using FuelStation.Data;
using FuelStation.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FuelStation.Controllers
{
    [TypeFilter(typeof(TimingLogAttribute))]
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
