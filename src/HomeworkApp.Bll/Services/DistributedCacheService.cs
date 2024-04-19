using HomeworkApp.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Bll.Services
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public Task<string> GetStringAsync(string key, CancellationToken token)
        {
            return _cache.GetStringAsync(key, token);
        }

        public Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token)
        {
            return _cache.SetStringAsync(key, value, options, token);
        }
    }

}
