# DeveloperHelper.Cache

This module is designed to simplify caching operations in your .NET applications. It provides support for both in-memory caching and distributed caching (Redis), making it easy to implement caching strategies in your applications.

## Features

- In-memory caching support
- Distributed caching (Redis) support
- Automatic cache expiration management
- Cache key management
- Cache state monitoring
- Cache clearing and refreshing
- Thread-safe operations
- Automatic serialization/deserialization

## Installation

```bash
dotnet add package DeveloperHelper.Cache
```

## Usage

### In-Memory Cache

```csharp
using DeveloperHelper.Cache;

// Configure cache service
services.AddMemoryCacheService();

// Cache usage
public class MyService
{
    private readonly ICacheService _cacheService;

    public MyService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Data> GetDataAsync(string key)
    {
        // Get data from cache
        var data = await _cacheService.GetAsync<Data>(key);
        
        if (data == null)
        {
            // If data not in cache, get from database and cache it
            data = await GetDataFromDatabase();
            await _cacheService.SetAsync(key, data, TimeSpan.FromMinutes(30));
        }

        return data;
    }
}
```

### Distributed Cache (Redis)

```csharp
using DeveloperHelper.Cache;

// Configure Redis cache service
services.AddDistributedCacheService(options =>
{
    options.ConnectionString = "localhost:6379";
    options.InstanceName = "MyApp:";
});

// Cache usage
public class MyService
{
    private readonly IDistributedCacheService _cacheService;

    public MyService(IDistributedCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Data> GetDataAsync(string key)
    {
        // Get data from distributed cache
        var data = await _cacheService.GetAsync<Data>(key);
        
        if (data == null)
        {
            // If data not in cache, get from database and cache it
            data = await GetDataFromDatabase();
            await _cacheService.SetAsync(key, data, TimeSpan.FromMinutes(30));
        }

        return data;
    }
}
```

## Features and Methods

### ICacheService

- `GetAsync<T>(string key)`: Get data from cache
- `SetAsync<T>(string key, T value, TimeSpan? expiration = null)`: Set data in cache
- `RemoveAsync(string key)`: Remove data from cache
- `ClearAsync()`: Clear all cache
- `ExistsAsync(string key)`: Check if data exists in cache

### IDistributedCacheService

- `GetAsync<T>(string key)`: Get data from distributed cache
- `SetAsync<T>(string key, T value, TimeSpan? expiration = null)`: Set data in distributed cache
- `RemoveAsync(string key)`: Remove data from distributed cache
- `ClearAsync()`: Clear all distributed cache
- `ExistsAsync(string key)`: Check if data exists in distributed cache

## Configuration

### Memory Cache Configuration

```csharp
services.AddMemoryCacheService(options =>
{
    options.SizeLimit = 1024; // Cache size limit
    options.CompactionPercentage = 0.2; // Compaction percentage
    options.EnableCompression = true; // Enable compression
    options.DefaultExpiration = TimeSpan.FromMinutes(30); // Default expiration time
});
```

### Distributed Cache Configuration

```csharp
services.AddDistributedCacheService(options =>
{
    options.ConnectionString = "localhost:6379";
    options.InstanceName = "MyApp:";
    options.DefaultExpiration = TimeSpan.FromMinutes(30);
    options.SyncTimeout = TimeSpan.FromSeconds(5);
    options.EnableCompression = true;
    options.EnableRetryPolicy = true;
    options.MaxRetries = 3;
});
```

## Example Usage Scenarios

### 1. Data Caching

```csharp
public class ProductService
{
    private readonly ICacheService _cacheService;
    private readonly IProductRepository _repository;

    public ProductService(ICacheService cacheService, IProductRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    public async Task<Product> GetProductAsync(int id)
    {
        var cacheKey = $"product:{id}";
        var product = await _cacheService.GetAsync<Product>(cacheKey);

        if (product == null)
        {
            product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                await _cacheService.SetAsync(cacheKey, product, TimeSpan.FromHours(1));
            }
        }

        return product;
    }
}
```

### 2. List Caching

```csharp
public class CategoryService
{
    private readonly ICacheService _cacheService;
    private readonly ICategoryRepository _repository;

    public CategoryService(ICacheService cacheService, ICategoryRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        const string cacheKey = "categories:all";
        var categories = await _cacheService.GetAsync<List<Category>>(cacheKey);

        if (categories == null)
        {
            categories = await _repository.GetAllAsync();
            await _cacheService.SetAsync(cacheKey, categories, TimeSpan.FromHours(24));
        }

        return categories;
    }
}
```

## Best Practices

1. **Cache Key Strategy**
   - Use meaningful and consistent key names
   - Add versioning (e.g., "v1:product:123")
   - Establish naming conventions

2. **Cache Duration**
   - Set appropriate durations based on data type
   - Use short durations for critical data
   - Use long durations for static data

3. **Cache Clearing**
   - Clear cache when data is updated
   - Clear entire cache for bulk updates
   - Use timers for regular cleanup

4. **Error Handling**
   - Log cache errors
   - Implement fallback mechanisms
   - Apply circuit breaker pattern

5. **Performance Optimization**
   - Use compression for large objects
   - Implement cache warming
   - Monitor cache hit rates

6. **Security**
   - Sanitize cache keys
   - Encrypt sensitive data
   - Implement proper access control

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 