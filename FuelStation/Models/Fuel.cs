using System.Collections.Generic;

namespace FuelStation.Models
{
    public class Fuel
    {
        //Id Топлива
        public int FuelID { get; set; }
        //Название вида топлива
        public string FuelType { get; set; }
        //Плотность вида топлива
        public float FuelDensity { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
        public Fuel()
        {
            Operations = new List<Operation>();

        }
    }
}
