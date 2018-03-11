using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuelStation.Data;
using FuelStation.ViewModels;
using FuelStation.Infrastructure.Filters;

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
        public IActionResult Index()
        {
            var fuelsContext = _context.Operations.Include(o => o.Fuel).Include(o => o.Tank);
            OperationViewModel operation = new OperationViewModel
            {
                FuelType = "",
                TankType=""
            };

            OperationsViewModel operations = new OperationsViewModel
            {
                Operations = fuelsContext,
                OperationViewModel=operation

            };
            return View(operations);
        }
        // Post: Operations
        [HttpPost]
        [SetToSession("Operation")]
        public IActionResult Index(OperationViewModel operation)
        {
            var fuelsContext = _context.Operations
                .Include(o => o.Fuel)
                .Include(o => o.Tank)
                .Where(o=>o.Tank.TankType.Contains(operation.TankType??"") 
                        & o.Fuel.FuelType.Contains(operation.FuelType??""));

            OperationsViewModel operations = new OperationsViewModel
            {
                Operations=fuelsContext,
                OperationViewModel = operation
            };



            return View(operations);
        }


    }
}
