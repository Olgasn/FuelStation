using System;
namespace Fuels.Models
{
    public class Operation
    {
        //ID операции
        public int OperationID { get; set; }
        //ID топлива
        public int? FuelID { get; set; }
        //ID емкости
        public int? TankID { get; set; }
        //Приход/Расход
        public float? Inc_Exp { get; set; }
        //Дата операции
        public DateTime Date { get; set; }
        //ссылка на виды топлива
        public virtual Fuel Fuel { get; set; }
        //ссылка на емкости
        public virtual Tank Tank { get; set; }

    }
}
