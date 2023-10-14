using FuelStation.DataLayer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.ViewModels
{
    public class TanksViewModel
    {
        public IEnumerable<Tank> Tanks { get; set; }

        //Свойство для фильтрации
        //Наименование емкости
        [Display(Name = "Наименование емкости")]
        public string TankType { get; set; }

        //Вес емкости
        [Display(Name = "Вес")]
        public float TankWeight { get; set; }

        //Объем емкости
        [Display(Name = "Объем")]
        public float TankVolume { get; set; }

        //Материал емкости
        [Display(Name = "Материал")]
        public string TankMaterial { get; set; }



        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
