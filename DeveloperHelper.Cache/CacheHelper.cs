using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using DeveloperHelper.Logging;

namespace DeveloperHelper.Cache;

/// <summary>
/// Cache functionality for DeveloperHelper library
/// </summary>
public static class CacheHelper
{
    private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions
    {
        SizeLimit = 1024, // Maximum number of items
        ExpirationScanFrequency = TimeSpan.FromMinutes(5),
        CompactionPercentage = 0.2 // Compact 20% of items when limit is reached
    });

    /// <summary>
    /// Sets a value in the memory cache
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    /// <param name="expiration">The expiration time</param>
    public static void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            var options = new MemoryCacheEntryOptions()
                .SetSize(1) // Each item counts as 1 unit
                .SetSlidingExpiration(expiration ?? TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(expiration ?? TimeSpan.FromHours(1))
                .RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
                {
                    // Log eviction if needed
                    if (reason == EvictionReason.Capacity)
                    {
                        LoggerHelper.LogWarning($"Cache item evicted due to capacity: {evictedKey}");
                    }
                });

            _cache.Set(key, value, options);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to set cache item: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets a value from the memory cache
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <returns>The cached value, or default if not found</returns>
    public static T? Get<T>(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            if (_cache.TryGetValue<T>(key, out var value))
            {
                return value;
            }
            return default;
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to get cache item: {ex.Message}");
            return default;
        }
    }

    /// <summary>
    /// Gets a value from the memory cache, or sets it if not found
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="factory">The factory function to create the value</param>
    /// <param name="expiration">The expiration time</param>
    /// <returns>The cached value</returns>
    public static T GetOrSet<T>(string key, Func<T> factory, TimeSpan? expiration = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(factory);

        try
        {
            T? value;
            if (_cache.TryGetValue(key, out value))
            {
                return value;
            }

            value = factory();
            Set(key, value, expiration);
            return value;
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to get or set cache item: {ex.Message}");
            return factory();
        }
    }

    /// <summary>
    /// Gets a value from the memory cache, or sets it if not found
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="factory">The factory function to create the value</param>
    /// <param name="expiration">The expiration time</param>
    /// <returns>The cached value</returns>
    public static async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(factory);

        try
        {
            T? value;
            if (_cache.TryGetValue(key, out value))
            {
                return value;
            }

            value = await factory();
            Set(key, value, expiration);
            return value;
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to get or set cache item: {ex.Message}");
            return await factory();
        }
    }

    /// <summary>
    /// Removes a value from the memory cache
    /// </summary>
    /// <param name="key">The cache key</param>
    public static void Remove(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            _cache.Remove(key);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to remove cache item: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Clears all values from the memory cache
    /// </summary>
    public static void Clear()
    {
        try
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0); // Compact 100% to clear all items
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to clear cache: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Checks if a value exists in the memory cache
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>True if the value exists, false otherwise</returns>
    public static bool Exists(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            return _cache.TryGetValue(key, out _);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to check cache item existence: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Updates a value in the memory cache if it exists
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="value">The new value</param>
    /// <param name="expiration">The expiration time</param>
    /// <returns>True if the value was updated, false otherwise</returns>
    public static bool Update<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (_cache is MemoryCache memoryCache)
        {
            if (!memoryCache.TryGetValue(key, out _))
            {
                return false;
            }
        }

        Set(key, value, expiration);
        return true;
    }

    /// <summary>
    /// Sets or updates a value in the memory cache
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to set or update</param>
    /// <param name="expiration">The expiration time</param>
    public static void SetOrUpdate<T>(string key, T value, TimeSpan? expiration = null)
    {
        Set(key, value, expiration);
    }
} 