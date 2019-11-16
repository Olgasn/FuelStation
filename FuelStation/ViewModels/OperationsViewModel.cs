using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.ViewModels
{
    public class OperationsViewModel
    {
        public IEnumerable<Operation> Operations { get;set;}        
        //Свойство для фильтрации
        public FilterOperationViewModel FilterOperationViewModel { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }

    }
}
