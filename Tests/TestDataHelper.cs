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
                    FuelDensity = 2.14F
                },
                new() {
                    FuelID=3,
                    FuelType = "Oil",
                    FuelDensity = 3.14F
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
    }
}
