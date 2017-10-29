using Fuels.Models;
using System.Collections.Generic;

namespace Fuels.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Tank> Tanks { get; set; }
        public IEnumerable<Fuel> Fuels { get; set; }
        public IEnumerable<OperationViewModel> Operations { get; set; }



    }

}
