using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Dal.Repositories.Interfaces
{
    public interface IDistributedCacheService
    {
        Task<string> GetStringAsync(string key, CancellationToken token);
        Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token);
    }

}
