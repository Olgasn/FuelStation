using System.Collections.Generic;

namespace Fuels.Models
{
    public class Tank
    {
        //ID емкости
        public int TankID { get; set; }
        //Тип емкости
        public string TankType { get; set; }
        //Вес емкости
        public float TankWeight { get; set; }
        //Объем емкости
        public float TankVolume { get; set; }
        //Материал емкости
        public string TankMaterial { get; set; }
        //ссылка на файл изображения емкости
        public string TankPicture { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }

    }
}
