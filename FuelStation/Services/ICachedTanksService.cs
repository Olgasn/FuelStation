using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.Services
{
    public interface ICachedTanksService
    {
        public IEnumerable<Tank> GetTanks(int rowsNumber = 20);
        public void AddTanks(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Tank> GetTanks(string cacheKey, int rowsNumber = 20);
    }
}
