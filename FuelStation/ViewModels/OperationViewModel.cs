using System;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.ViewModels
{
    public class OperationViewModel
    {
        //ID операции
        public int OperationID { get; set; }
        //название топлива
        [Display(Name = "Топливо")]
        public string FuelType { get; set; }
        //название емкости
        [Display(Name = "Емкость")]
        public string TankType { get; set; }
        //Приход/Расход
        [Display(Name = "+Приход/-Расход")]
        public float? Inc_Exp { get; set; }
        //Дата операции
        [Display(Name = "Дата операции")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


    }
}
