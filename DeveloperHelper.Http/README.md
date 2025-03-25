# DeveloperHelper Library

A comprehensive .NET library providing essential developer tools and utilities for common programming tasks.

## Table of Contents
- [Installation](#installation)
- [Modules](#modules)
  - [Core](#core)
  - [Security](#security)
  - [Logging](#logging)
  - [Cache](#cache)
  - [Database](#database)
  - [HTTP](#http)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package DeveloperHelper.Core
dotnet add package DeveloperHelper.Security
dotnet add package DeveloperHelper.Logging
dotnet add package DeveloperHelper.Cache
dotnet add package DeveloperHelper.Database
dotnet add package DeveloperHelper.Http
```

## Modules

### Core

The Core module provides essential utility functions for common programming tasks.

```csharp
// String Operations
string slug = "Hello World!".ToSlug(); // "hello-world"
string camelCase = "hello_world".ToCamelCase(); // "helloWorld"
string pascalCase = "hello_world".ToPascalCase(); // "HelloWorld"
string snakeCase = "HelloWorld".ToSnakeCase(); // "hello_world"

// Number Parsing
int? number = "123".ParseInt();
decimal? amount = "123.45".ParseDecimal();
DateTime? date = "2024-03-20".ParseDateTime();
bool? flag = "true".ParseBool();

// File Operations
FileHelper.CreateDirectoryIfNotExists("path/to/dir");
string content = FileHelper.ReadAllText("file.txt");
FileHelper.WriteAllText("file.txt", "content");
bool exists = FileHelper.FileExists("file.txt");
bool isDirectory = FileHelper.IsDirectory("path/to/dir");

// JSON Operations
var json = JsonHelper.Serialize(new { Name = "John", Age = 30 });
var obj = JsonHelper.Deserialize<User>(json);
var prettyJson = JsonHelper.PrettyPrint(json);

// XML Operations
var xml = XmlHelper.Serialize(new { Name = "John", Age = 30 });
var xmlObj = XmlHelper.Deserialize<User>(xml);
var prettyXml = XmlHelper.PrettyPrint(xml);

// CSV Operations
var csv = CsvHelper.Serialize(users);
var users = CsvHelper.Deserialize<User>(csv);

// Validation
bool isValidEmail = "john@example.com".IsValidEmail();
bool isValidUrl = "https://example.com".IsValidUrl();
bool isValidPhone = "+1234567890".IsValidPhone();
bool isValidIpAddress = "192.168.1.1".IsValidIpAddress();

// Formatting
string formattedNumber = 1234567.89.FormatNumber("N2"); // "1,234,567.89"
string formattedCurrency = 1234.56.FormatCurrency("USD"); // "$1,234.56"
string formattedDate = DateTime.Now.FormatDate("yyyy-MM-dd"); // "2024-03-20"
string formattedTime = DateTime.Now.FormatTime("HH:mm:ss"); // "14:30:45"

// String Manipulation
string truncated = "Hello World".Truncate(5); // "Hello..."
string reversed = "Hello".Reverse(); // "olleH"
string[] words = "Hello World".SplitWords(); // ["Hello", "World"]
string cleaned = "Hello   World".CleanWhitespace(); // "Hello World"

// Collection Operations
var distinct = list.DistinctBy(x => x.Id);
var grouped = list.GroupBy(x => x.Category);
var paginated = list.Paginate(page: 1, pageSize: 10);
var shuffled = list.Shuffle();
```

### Security

The Security module provides essential security features including password hashing, JWT token management, and data encryption.

```csharp
// Password Hashing
string hashedPassword = SecurityHelper.HashPassword("myPassword123");
bool isValid = SecurityHelper.VerifyPassword("myPassword123", hashedPassword);

// JWT Token Generation
var claims = new Dictionary<string, string>
{
    { "userId", "123" },
    { "role", "admin" }
};
string token = SecurityHelper.GenerateJwtToken(claims, "your-secret-key", TimeSpan.FromHours(1));

// Token Validation
var principal = SecurityHelper.ValidateJwtToken(token, "your-secret-key");

// Data Encryption
string encrypted = SecurityHelper.Encrypt("sensitive data", "encryption-key");
string decrypted = SecurityHelper.Decrypt(encrypted, "encryption-key");

// HTML Sanitization
string cleanHtml = SecurityHelper.SanitizeHtml("<script>alert('xss')</script>Hello");

// SQL Sanitization
string cleanSql = SecurityHelper.SanitizeSql("SELECT * FROM Users WHERE id = '1; DROP TABLE Users;'");
```

### Logging

The Logging module provides a flexible logging system with multiple log levels and formatting options.

```csharp
// Configure Logger
LoggerHelper.Configure(LogLevel.Debug);

// Basic Logging
LoggerHelper.LogDebug("Debug message");
LoggerHelper.LogInformation("Info message");
LoggerHelper.LogWarning("Warning message");

// Error Logging with Exception
try
{
    // Some operation
}
catch (Exception ex)
{
    LoggerHelper.LogError("Operation failed", ex);
}

// Critical Logging
LoggerHelper.LogCritical("System failure detected");

// Custom Log Level
LoggerHelper.Log(LogLevel.Information, "Custom message");
```

### Cache

The Cache module provides in-memory caching capabilities with flexible options.

```csharp
// Store Value
CacheHelper.Set("key", "value", TimeSpan.FromMinutes(30));

// Retrieve Value
string value = CacheHelper.Get<string>("key");

// Get or Set Pattern
var result = CacheHelper.GetOrSet("key", () => "computed-value", TimeSpan.FromHours(1));

// Async Operations
var asyncResult = await CacheHelper.GetOrSetAsync("key", 
    async () => await ComputeValueAsync(), 
    TimeSpan.FromHours(1));

// Check Existence
bool exists = CacheHelper.Exists("key");

// Remove Item
CacheHelper.Remove("key");

// Clear Cache
CacheHelper.Clear();
```

### Database

The Database module simplifies database operations with support for various operations.

```csharp
// Execute Query
var results = DatabaseHelper.ExecuteQuery<User>(
    "SELECT * FROM Users WHERE Age > @Age",
    new { Age = 18 }
);

// Execute Stored Procedure
var result = DatabaseHelper.ExecuteStoredProcedure<OrderResult>(
    "CreateOrder",
    new { CustomerId = 1, Amount = 100.50 }
);

// Execute Command
int rowsAffected = DatabaseHelper.ExecuteCommand(
    "UPDATE Products SET Stock = Stock - 1 WHERE Id = @Id",
    new { Id = 123 }
);

// Transaction Support
using (var transaction = DatabaseHelper.BeginTransaction())
{
    try
    {
        DatabaseHelper.ExecuteCommand("INSERT INTO Orders...");
        DatabaseHelper.ExecuteCommand("UPDATE Inventory...");
        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

### HTTP

This module provides a robust and flexible HTTP client wrapper for making HTTP requests in your .NET applications. It includes features like retry policies, circuit breakers, and automatic error handling.

## Features

- HTTP request/response handling
- Retry policies with exponential backoff
- Circuit breaker pattern implementation
- Automatic error handling and logging
- Request/response validation
- Custom header management
- Timeout configuration
- Request/response compression
- Cookie handling
- Proxy support

## Installation

```bash
dotnet add package DeveloperHelper.Http
```

## Usage

### Basic Configuration

```csharp
using DeveloperHelper.Http;

// Configure HTTP service
services.AddHttpService(options =>
{
    options.BaseUrl = "https://api.example.com";
    options.DefaultTimeout = TimeSpan.FromSeconds(30);
    options.EnableRetryPolicy = true;
    options.MaxRetries = 3;
    options.EnableCircuitBreaker = true;
});
```

### Basic HTTP Operations

```csharp
public class ApiService
{
    private readonly IHttpService _httpService;

    public ApiService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _httpService.GetAsync<User>($"/users/{id}");
    }

    public async Task<User> CreateUserAsync(User user)
    {
        return await _httpService.PostAsync<User>("/users", user);
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        await _httpService.PutAsync($"/users/{id}", user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _httpService.DeleteAsync($"/users/{id}");
    }
}
```

### Advanced Usage

```csharp
public class ApiService
{
    private readonly IHttpService _httpService;

    public ApiService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<List<Product>> GetProductsAsync(string category, int page = 1, int pageSize = 10)
    {
        var options = new HttpRequestOptions
        {
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer token" },
                { "Custom-Header", "Value" }
            },
            QueryParameters = new Dictionary<string, string>
            {
                { "category", category },
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            },
            Timeout = TimeSpan.FromSeconds(60),
            EnableCompression = true
        };

        return await _httpService.GetAsync<List<Product>>("/products", options);
    }

    public async Task<FileResponse> DownloadFileAsync(string fileId)
    {
        var options = new HttpRequestOptions
        {
            ResponseType = ResponseType.Stream,
            EnableCompression = false
        };

        return await _httpService.GetAsync<FileResponse>($"/files/{fileId}", options);
    }
}
```

## Features and Methods

### IHttpService

- `GetAsync<T>(string endpoint, HttpRequestOptions options = null)`: Send GET request
- `PostAsync<T>(string endpoint, object data, HttpRequestOptions options = null)`: Send POST request
- `PutAsync<T>(string endpoint, object data, HttpRequestOptions options = null)`: Send PUT request
- `DeleteAsync<T>(string endpoint, HttpRequestOptions options = null)`: Send DELETE request
- `PatchAsync<T>(string endpoint, object data, HttpRequestOptions options = null)`: Send PATCH request
- `SendAsync<T>(HttpRequestMessage request, HttpRequestOptions options = null)`: Send custom HTTP request

### HttpRequestOptions

- `Headers`: Custom HTTP headers
- `QueryParameters`: URL query parameters
- `Timeout`: Request timeout
- `EnableCompression`: Enable request/response compression
- `ResponseType`: Response type (Json, Stream, etc.)
- `RetryPolicy`: Custom retry policy
- `CircuitBreakerPolicy`: Custom circuit breaker policy

## Configuration

### HTTP Service Configuration

```csharp
services.AddHttpService(options =>
{
    options.BaseUrl = "https://api.example.com";
    options.DefaultTimeout = TimeSpan.FromSeconds(30);
    options.EnableRetryPolicy = true;
    options.MaxRetries = 3;
    options.RetryInterval = TimeSpan.FromSeconds(1);
    options.EnableCircuitBreaker = true;
    options.CircuitBreakerThreshold = 5;
    options.CircuitBreakerDuration = TimeSpan.FromSeconds(30);
    options.EnableCompression = true;
    options.DefaultHeaders = new Dictionary<string, string>
    {
        { "User-Agent", "MyApp/1.0" }
    };
});
```

## Example Usage Scenarios

### 1. API Client Implementation

```csharp
public class WeatherApiClient
{
    private readonly IHttpService _httpService;

    public WeatherApiClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<WeatherData> GetWeatherAsync(string city)
    {
        var options = new HttpRequestOptions
        {
            QueryParameters = new Dictionary<string, string>
            {
                { "city", city },
                { "units", "metric" }
            }
        };

        return await _httpService.GetAsync<WeatherData>("/weather", options);
    }

    public async Task<Forecast> GetForecastAsync(string city, int days)
    {
        var options = new HttpRequestOptions
        {
            QueryParameters = new Dictionary<string, string>
            {
                { "city", city },
                { "days", days.ToString() }
            }
        };

        return await _httpService.GetAsync<Forecast>("/forecast", options);
    }
}
```

### 2. File Upload/Download

```csharp
public class FileService
{
    private readonly IHttpService _httpService;

    public FileService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        var options = new HttpRequestOptions
        {
            Content = content,
            EnableCompression = false
        };

        var response = await _httpService.PostAsync<UploadResponse>("/files", options);
        return response.FileId;
    }

    public async Task<byte[]> DownloadFileAsync(string fileId)
    {
        var options = new HttpRequestOptions
        {
            ResponseType = ResponseType.Stream,
            EnableCompression = false
        };

        var response = await _httpService.GetAsync<FileResponse>($"/files/{fileId}", options);
        return response.Content;
    }
}
```

## Best Practices

1. **Error Handling**
   - Implement proper error handling
   - Use retry policies for transient failures
   - Handle circuit breaker states
   - Log errors appropriately

2. **Performance**
   - Use compression for large payloads
   - Implement proper timeout values
   - Use connection pooling
   - Monitor response times

3. **Security**
   - Use HTTPS for all requests
   - Implement proper authentication
   - Validate SSL certificates
   - Sanitize input data

4. **Monitoring**
   - Log request/response details
   - Monitor circuit breaker states
   - Track retry attempts
   - Monitor response times

5. **Configuration**
   - Use environment-specific settings
   - Implement proper timeout values
   - Configure retry policies
   - Set up circuit breaker thresholds

## License

This project is licensed under the MIT License - see the LICENSE file for details. 