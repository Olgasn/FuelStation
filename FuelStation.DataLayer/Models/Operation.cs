using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelStation.DataLayer.Models
{
    public class Operation
    {
        //ID операции
        [Key]
        [Display(Name = "Код операции")]
        public int OperationID { get; set; }

        //ID топлива
        [Display(Name = "Код топлива")]
        [ForeignKey("Fuel")]
        public int FuelID { get; set; }

        //ID емкости
        [Display(Name = "Код емкости")]
        [ForeignKey("Tank")]
        public int TankID { get; set; }

        //Приход/Расход
        [Display(Name = "+Приход/-Расход")]
        public float? Inc_Exp { get; set; }

        //Дата операции
        [Display(Name = "Дата операции")]
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }

        //ссылка по внешнему ключу FuelID на Fuel
        public virtual Fuel Fuel { get; set; }
        //ссылка по внешнему ключу TankID на Tank
        public virtual Tank Tank { get; set; }

    }
}
