using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Bll.Services.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetStringAsync(string key, CancellationToken token);
        Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token);
    }
}
