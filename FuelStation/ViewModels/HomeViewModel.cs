using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Tank> Tanks { get; set; }
        public IEnumerable<Fuel> Fuels { get; set; }
        public IEnumerable<OperationViewModel> Operations { get; set; }

    }

}
