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
    }
}
