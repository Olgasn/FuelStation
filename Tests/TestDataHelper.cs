using FuelStation.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class TestDataHelper
    {
        public static List<Fuel> GetFakeFuelsList()
        {
            return
            [
                new() {
                    FuelID=1,
                    FuelType = "Petrol",
                    FuelDensity = 3.14F
                },
                new() {
                    FuelID=2,
                    FuelType = "Kerosene",
                    FuelDensity = 2.16F
                },
                new() {
                    FuelID=3,
                    FuelType = "Oil",
                    FuelDensity = 3.2F
                }
            ];
        }

        public static List<Tank> GetFakeTanksList()
        {
            return
            [
                new() {
                    TankID=1,
                    TankType = "Small tank",
                    TankMaterial ="Silver",
                    TankVolume = 300.4F,
                    TankWeight = 123.14F
                },
                new() {
                    TankID=2,
                    TankType = "Medium tank",
                    TankMaterial ="Copper",
                    TankVolume = 3000.5F,
                    TankWeight = 1023.14F
                },
                new() {
                    TankID=3,
                    TankType = "Big tank",
                    TankMaterial ="Steel",
                    TankVolume = 30000.7F,
                    TankWeight = 2024.14F
                }
            ];
        }


        public static List<Operation> GetFakeOperationsList()
        {
            int operations_number = 9;
            int tanks_number = GetFakeTanksList().Count;
            int fuels_number = GetFakeFuelsList().Count;
            Random randObj = new(1);
            List<Operation> operations = [];

            //Заполнение таблицы операций
            for (int operationID = 1; operationID <= operations_number; operationID++)
            {
                int tankID = randObj.Next(1, tanks_number);
                int fuelID = randObj.Next(1, fuels_number);
                int inc_exp = randObj.Next(200) - 100;
                DateTime today = DateTime.Now.Date;
                DateTime operationdate = today.AddDays(-operationID);
                operations.Add(new Operation
                { 
                    OperationID = operationID,
                    TankID = tankID, 
                    FuelID = fuelID, 
                    Inc_Exp = inc_exp, 
                    Date = operationdate,
                    Fuel = GetFakeFuelsList().SingleOrDefault(m => m.FuelID == fuelID),
                    Tank = GetFakeTanksList().SingleOrDefault(m => m.TankID == tankID)
                });
            }



            return operations;

        }

    }
}
