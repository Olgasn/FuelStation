using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.Services
{
    interface ICachedTanksService
    {
        IEnumerable<Tank> GetTanks();
        void AddTanks(string cacheKey);
        IEnumerable<Tank> GetTanks(string cacheKey);
    }
}
