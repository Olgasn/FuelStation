using FuelStation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.ViewModels
{
    public class FuelsViewModel
    {
        public IEnumerable<Fuel> Fuels { get; set; }
        //Свойство для фильтрации
        //название топлива
        [Display(Name = "Топливо")]
        public string FuelType { get; set; }
        //Плотность вида топлива
        [Display(Name = "Плотность ")]
        public float FuelDensity { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
