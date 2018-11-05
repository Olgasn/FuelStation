using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuelStation.Models
{
    public class Tank
    {
        //ID емкости
        [Key]
        [Display(Name = "Код емкости")]
        public int TankID { get; set; }

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

        //ссылка на файл изображения емкости
        [Display(Name = "Изображение")]
        public string TankPicture { get; set; }

        //Коллекция объектов Operation, связанных с моделью
        public virtual ICollection<Operation> Operations { get; set; }

        public Tank()
        {
            Operations = new List<Operation>();

        }

    }
}
