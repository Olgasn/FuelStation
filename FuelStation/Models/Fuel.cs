using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.Models
{
    public class Fuel
    {
        //Id Топлива
        [Display(Name = "Код топлива")]
        public int FuelID { get; set; }
        //Название вида топлива
        [Display(Name = "Топливо")]
        public string FuelType { get; set; }
        //Плотность вида топлива
        [Display(Name = "Плотность топлива")]
        public float FuelDensity { get; set; }
        public ICollection<Operation> Operations { get; set; }
        public Fuel()
        {
            Operations = new List<Operation>();

        }
    }
}
