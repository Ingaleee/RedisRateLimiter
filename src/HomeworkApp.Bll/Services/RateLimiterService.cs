using HomeworkApp.Bll.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Bll.Services
{
    public class RateLimiterService
    {
        private readonly ICacheService _cache;
        private const int RequestLimit = 100;
        private const string Prefix = "RateLimit-";

        public RateLimiterService(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task<bool> IsRequestAllowed(string ipAddress)
        {
            var cacheKey = $"{Prefix}{ipAddress}";
            var currentValue = await _cache.GetStringAsync(cacheKey, CancellationToken.None);
            var requestCount = currentValue != null ? int.Parse(currentValue) : 0;

            if (requestCount >= RequestLimit)
            {
                return false;
            }

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            await _cache.SetStringAsync(cacheKey, (requestCount + 1).ToString(), options, CancellationToken.None);
            return true;
        }
    }

}
