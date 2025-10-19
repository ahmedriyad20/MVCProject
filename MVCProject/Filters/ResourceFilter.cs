using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace MVCProject.Filters
{
    public class ResourceFilter : Attribute, IResourceFilter
    {
        private readonly string _cacheKey;
        private readonly int _cacheDurationSeconds;

        public ResourceFilter(string cacheKey = "default", int cacheDurationSeconds = 60)
        {
            _cacheKey = cacheKey;
            _cacheDurationSeconds = cacheDurationSeconds;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Get the cache service
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            
            if (cache == null)
                return;

            // Try to get cached result
            var cacheKey = $"{_cacheKey}_{context.HttpContext.Request.Path}";
            
            if (cache.TryGetValue(cacheKey, out IActionResult cachedResult))
            {
                Console.WriteLine($"✅ Returning cached response for: {cacheKey}");
                
                // Short-circuit the pipeline - return cached result
                context.Result = cachedResult;
            }
            else
            {
                Console.WriteLine($"🔄 Cache miss for: {cacheKey}");
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Cache the result after execution
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            
            if (cache == null || context.Result == null)
                return;

            var cacheKey = $"{_cacheKey}_{context.HttpContext.Request.Path}";
            
            // Store result in cache
            cache.Set(cacheKey, context.Result, TimeSpan.FromSeconds(_cacheDurationSeconds));
            
            Console.WriteLine($"💾 Cached response for: {cacheKey} (expires in {_cacheDurationSeconds}s)");
        }
    }
}
