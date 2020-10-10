using FuelStation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.Services
{
    interface ICachedTanksService
    {
        IEnumerable<Tank> GetTanks();
        void AddTanks(string cacheKey);
        IEnumerable<Tank> GetTanks(string cacheKey);
    }
}
