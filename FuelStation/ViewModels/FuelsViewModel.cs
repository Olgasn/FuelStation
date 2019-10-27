using FuelStation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.ViewModels
{
    public class FuelsViewModel
    {
        public IEnumerable<Fuel> Fuels { get; set; }
        //Свойство для фильтрации
        //название топлива
        public string FuelType { get; set; }
        //Плотность вида топлива
       public float FuelDensity { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
