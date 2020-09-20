using System;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.Models
{
    public class Operation
    {
        //ID операции
        [Key]
        [Display(Name = "Код операции")]
        public int OperationId { get; set; }
        //ID топлива
        [Display(Name = "Код топлива")]
        public int FuelId { get; set; }
        //ID емкости
        [Display(Name = "Код емкости")]
        public int TankId { get; set; }
        //Приход/Расход
        [Display(Name = "+Приход/-Расход")]
        public float? Inc_Exp { get; set; }
        //Дата операции
        [Display(Name = "Дата операции")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        //ссылка на виды топлива
        public Fuel Fuel { get; set; }
        //ссылка на емкости
        public Tank Tank { get; set; }

    }
}
