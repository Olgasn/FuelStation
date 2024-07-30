using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.DataLayer.Models
{
    public class Fuel
    {
        //Id Топлива
        [Key]
        [Display(Name = "Код топлива")]
        public int FuelID { get; set; }

        //Наименование вида топлива
        [Display(Name = "Наименование топлива")]
        [Required]
        public string FuelType { get; set; }

        //Плотность вида топлива
        [Display(Name = "Плотность топлива")]
        public float FuelDensity { get; set; }

        //Коллекция объектов Operation, связанных с моделью
        public virtual ICollection<Operation> Operations { get; set; }

        public Fuel()
        {
            Operations = [];

        }
    }
}
