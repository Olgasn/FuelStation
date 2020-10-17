using FuelStation.Data;
using FuelStation.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace FuelStation.Services
{
    // Класс выборки 10 записей из всех таблиц 
    public class OperationService: IOperationService
    {
        private readonly FuelsContext _context;
        public OperationService(FuelsContext context)
        {
            _context = context;
        }
        public HomeViewModel GetHomeViewModel(int numberRows=10)
            {
            var fuels = _context.Fuels.Take(numberRows).ToList();
            var tanks = _context.Tanks.Take(numberRows).ToList();
            List<OperationViewModel> operations = _context.Operations
                .OrderByDescending(d => d.Date)
                .Select(t => new OperationViewModel
                {
                    OperationID = t.OperationID,
                    FuelType = t.Fuel.FuelType,
                    TankType = t.Tank.TankType,
                    Inc_Exp = t.Inc_Exp,
                    Date = t.Date
                })
                .Take(10)
                .ToList();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Tanks = tanks,
                Fuels = fuels,
                Operations = operations
            };
            return homeViewModel;
        }

    }
}
