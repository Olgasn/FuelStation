using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuelStation.Data;
using FuelStation.ViewModels;
using FuelStation.Infrastructure.Filters;
using FuelStation.Infrastructure;

namespace FuelStation.Controllers
{
    [TypeFilter(typeof(TimingLogAttribute))]
    [ExceptionFilter]
    public class OperationsController : Controller
    {
        private readonly FuelsContext _context;
        private OperationViewModel _operation=new OperationViewModel
        {
            FuelType="",
            TankType=""
        };

        public OperationsController(FuelsContext context)
        {
            _context = context;


        }


        // GET: Operations
        public IActionResult Index()
        {
            var session = HttpContext.Session.Get("Operation");
            if (session != null)
            {
                _operation = Transformations.DictionaryToObject<OperationViewModel>(session);
            }
            var fuelsContext = _context.Operations
                .Include(o => o.Fuel)
                .Include(o => o.Tank)
                .Where(o => o.Tank.TankType.Contains(_operation.TankType ?? "")
                        & o.Fuel.FuelType.Contains(_operation.FuelType ?? ""));

            OperationsViewModel operations = new OperationsViewModel
            {
                Operations = fuelsContext,
                OperationViewModel=_operation

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
