using Microsoft.Extensions.Caching.Memory;

namespace Sorted.Api.Extensions
{
    public static class MemoryCacheExtension
    {
        public static TItem SetCache<TItem>(this IMemoryCache cache, object key, TItem value, MemoryCacheEntryOptions options)
        {
            using (var entry = cache.CreateEntry(key))
            {
                if (options != null)
                {
                    entry.SetOptions(options);
                }

                entry.Value = value;
            }

            return value;
        }
    }
}
