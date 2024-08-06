﻿using FuelStation.Data;
using FuelStation.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelStation.Services
{
    public class CachedTanksService(FuelsContext dbContext, IMemoryCache memoryCache) : ICachedTanksService
    {
        private readonly FuelsContext _dbContext = dbContext;
        private readonly IMemoryCache _memoryCache = memoryCache;

        // получение списка емкостей из базы
        public IEnumerable<Tank> GetTanks(int rowsNumber = 20)
        {
            return _dbContext.Tanks.Take(rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddTanks(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Tank> tanks = _dbContext.Tanks.Take(rowsNumber).ToList();
            if (tanks != null)
            {
                _memoryCache.Set(cacheKey, tanks, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
        public IEnumerable<Tank> GetTanks(string cacheKey, int rowsNumber = 20)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Tank> tanks))
            {
                tanks = _dbContext.Tanks.Take(rowsNumber).ToList();
                if (tanks != null)
                {
                    _memoryCache.Set(cacheKey, tanks,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return tanks;
        }

    }
}
