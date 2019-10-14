using FuelStation.Data;
using FuelStation.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Services
{
    public class CachedTanksService
    {
        private FuelsContext db;
        private IMemoryCache cache;
        private int _rowsNumber;

        public CachedTanksService(FuelsContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
        }

        public IEnumerable<Tank> GetTanks()
        {
            return db.Tanks.Take(_rowsNumber).ToList();
        }

        public void AddTanks(string cacheKey)
        {
            IEnumerable<Tank> tanks = db.Tanks.Take(_rowsNumber);

            cache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        }

        public IEnumerable<Tank> GetTanks(string cacheKey)
        {
            IEnumerable<Tank> tanks = null;
            if (!cache.TryGetValue(cacheKey, out tanks))
            {
                tanks = db.Tanks.Take(_rowsNumber).ToList();
                if (tanks != null)
                {
                    cache.Set(cacheKey, tanks,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return tanks;
        }

    }
}

