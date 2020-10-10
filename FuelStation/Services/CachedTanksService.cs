using FuelStation.Data;
using FuelStation.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Services
{
    public class CachedTanksService: ICachedTanksService
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
        // получение списка емкостей из базы
        public IEnumerable<Tank> GetTanks()
        {
            return db.Tanks.Take(_rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddTanks(string cacheKey)
        {
            IEnumerable<Tank> tanks = null;
            tanks = db.Tanks.Take(_rowsNumber).ToList();
            if (tanks != null)
            {
                cache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
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

