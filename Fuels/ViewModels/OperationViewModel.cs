using System;

namespace Fuels.ViewModels
{
    public class OperationViewModel
    {
        //ID операции
        public int OperationID { get; set; }
        //ID топлива
        public string FuelType { get; set; }
        //ID емкости
        public string TankType { get; set; }
        //Приход/Расход
        public float? Inc_Exp { get; set; }
        //Дата операции
        public DateTime Date { get; set; }

    }
}
