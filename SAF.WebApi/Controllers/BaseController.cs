using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace SAF.WebApi.Controllers
{
    public class BaseController : Controller
    {
        IDistributedCache _cache;

        readonly DistributedCacheEntryOptions cacheOptions =
                          new DistributedCacheEntryOptions();

        public BaseController(
            IDistributedCache cache)
        {
            _cache = cache;

            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        }

        protected string GetCacheData(string key)
        {
            return _cache.GetString(key);
        }

        protected long PessoaId
        {
            get
            {
                return long.Parse(User?.Claims?.FirstOrDefault(c => c.Type == "PessoaId")?.Value);
            }
        }        

        protected async Task SetCacheData(string key, string value)
        {
            await _cache.SetStringAsync(key, value, cacheOptions);
        }
    }
}
