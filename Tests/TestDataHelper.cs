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
                    FuelType = "John Doe",
                    FuelDensity = 2

                },
                new() {
                    FuelID=2,
                    FuelType = "Mark Luther",
                    FuelDensity = 3

                }
            ];
        }
    }
}
