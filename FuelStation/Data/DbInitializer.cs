using FuelStation.Models;
using System;
using System.Linq;
//Класс для инициализации базы данных путем заполнения ее таблиц тестовым набором записей
namespace FuelStation.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FuelsContext db)
        {
            db.Database.EnsureCreated();

            // Проверка занесены ли емкости
            if (db.Tanks.Any())
            {
                return;   // База данных инициализирована
            }

            int tanks_number = 55;
            int fuels_number = 55;
            int operations_number = 600;
            string tankType;
            string tankMaterial;
            float tankWeight;
            float tankVolume;
            string fuelType;
            float fuelDensity;

            Random randObj = new(1);

            //Заполнение таблицы емкостей
            string[] tank_voc = ["Цистерна_", "Ведро_", "Бак_", "Фляга_", "Стакан_", "Танкер_"];//словарь названий емкостей
            string[] material_voc = ["Сталь", "Платина", "Алюминий", "ПЭТ", "Чугун", "Алюминий", "Сталь", "Дерево"];//словарь названий видов топлива
            int count_tank_voc = tank_voc.GetLength(0);
            int count_material_voc = material_voc.GetLength(0);
            for (int tankId = 1; tankId <= tanks_number; tankId++)
            {
                tankType = tank_voc[randObj.Next(count_tank_voc)] + tankId.ToString();
                tankMaterial = material_voc[randObj.Next(count_material_voc)];
                tankWeight = 500 * (float)randObj.NextDouble();
                tankVolume = 200 * (float)randObj.NextDouble();
                db.Tanks.Add(new Tank { TankType = tankType, TankWeight = tankWeight, TankVolume = tankVolume, TankMaterial = tankMaterial });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            //Заполнение таблицы видов топлива
            string[] fuel_voc = ["Нефть_", "Бензин_", "Керосин_", "Мазут_", "Спирт_", "Дизель_"];
            int count_fuel_voc = fuel_voc.GetLength(0);
            for (int fuelId = 1; fuelId <= fuels_number; fuelId++)
            {
                fuelType = fuel_voc[randObj.Next(count_fuel_voc)] + fuelId.ToString();
                fuelDensity = 2 * (float)randObj.NextDouble();
                db.Fuels.Add(new Fuel { FuelType = fuelType, FuelDensity = fuelDensity });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            //Заполнение таблицы операций
            for (int operationId = 1; operationId <= operations_number; operationId++)
            {
                int tankId = randObj.Next(1, tanks_number - 1);
                int fuelId = randObj.Next(1, fuels_number - 1);
                int inc_exp = randObj.Next(200) - 100;
                DateTime today = DateTime.Now.Date;
                DateTime operationdate = today.AddDays(-operationId);
                db.Operations.Add(new Operation { TankId = tankId, FuelId = fuelId, Inc_Exp = inc_exp, Date = operationdate });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();
        }

    }

}


