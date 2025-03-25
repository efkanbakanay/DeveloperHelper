# DeveloperHelper.Logging

This module provides a flexible and powerful logging system for your .NET applications. It supports multiple log levels, structured logging, and various output targets including console, file, and external logging services.

## Table of Contents
- [Installation](#installation)
- [Modules](#modules)
  - [Core](#core)
  - [Security](#security)
  - [Logging](#logging)
  - [Cache](#cache)
  - [Database](#database)
  - [HTTP](#http)
- [Features](#features)
- [Usage](#usage)
- [Configuration](#configuration)
- [Best Practices](#best-practices)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Features

- Multiple log levels (Debug, Information, Warning, Error, Critical)
- Structured logging support
- Console logging
- File logging with rotation
- External logging service integration
- Log filtering and enrichment
- Performance logging
- Request/Response logging
- Exception logging
- Correlation ID tracking

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

The HTTP module provides a simplified interface for making HTTP requests.

```csharp
// GET Request
var response = await HttpClientHelper.GetAsync<UserResponse>("https://api.example.com/users/1");

// POST Request
var user = new User { Name = "John", Email = "john@example.com" };
var result = await HttpClientHelper.PostAsync<CreateUserResponse>(
    "https://api.example.com/users",
    user
);

// PUT Request
await HttpClientHelper.PutAsync(
    "https://api.example.com/users/1",
    new { Name = "Updated Name" }
);

// DELETE Request
await HttpClientHelper.DeleteAsync("https://api.example.com/users/1");

// Custom Headers
var options = new Action<HttpRequestOptions>(opts =>
{
    opts.Headers.Add("Authorization", "Bearer token");
    opts.Headers.Add("Custom-Header", "Value");
});

var response = await HttpClientHelper.GetAsync<UserResponse>(
    "https://api.example.com/users/1",
    options
);
```

## Usage

### Basic Configuration

```csharp
using DeveloperHelper.Logging;

// Configure logging service
services.AddLoggingService(options =>
{
    options.MinimumLevel = LogLevel.Information;
    options.EnableConsoleLogging = true;
    options.EnableFileLogging = true;
    options.FilePath = "logs/app.log";
    options.EnableStructuredLogging = true;
});
```

### Basic Logging

```csharp
public class UserService
{
    private readonly ILoggingService _loggingService;

    public UserService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<User> CreateUserAsync(UserRegistration registration)
    {
        try
        {
            _loggingService.LogInformation("Creating new user: {Username}", registration.Username);

            var user = await CreateUserInDatabase(registration);

            _loggingService.LogInformation(
                "User created successfully: {UserId}, {Username}",
                user.Id,
                user.Username
            );

            return user;
        }
        catch (Exception ex)
        {
            _loggingService.LogError(
                ex,
                "Failed to create user: {Username}",
                registration.Username
            );
            throw;
        }
    }
}
```

### Performance Logging

```csharp
public class OrderService
{
    private readonly ILoggingService _loggingService;

    public OrderService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<Order> ProcessOrderAsync(Order order)
    {
        using (_loggingService.BeginScope("Processing order {OrderId}", order.Id))
        {
            using (var timer = _loggingService.TimeOperation("Order processing"))
            {
                try
                {
                    await ValidateOrder(order);
                    await ProcessPayment(order);
                    await UpdateInventory(order);
                    await SendConfirmation(order);

                    _loggingService.LogInformation(
                        "Order processed successfully: {OrderId}, Total: {Total}",
                        order.Id,
                        order.Total
                    );

                    return order;
                }
                catch (Exception ex)
                {
                    _loggingService.LogError(
                        ex,
                        "Order processing failed: {OrderId}",
                        order.Id
                    );
                    throw;
                }
            }
        }
    }
}
```

## Features and Methods

### ILoggingService

- `LogDebug(string message, params object[] args)`: Log debug message
- `LogInformation(string message, params object[] args)`: Log information message
- `LogWarning(string message, params object[] args)`: Log warning message
- `LogError(Exception ex, string message, params object[] args)`: Log error message
- `LogCritical(Exception ex, string message, params object[] args)`: Log critical message
- `BeginScope(string messageFormat, params object[] args)`: Create a new logging scope
- `TimeOperation(string operationName)`: Time an operation and log its duration
- `WithCorrelationId(string correlationId)`: Add correlation ID to log context

## Configuration

### Logging Service Configuration

```csharp
services.AddLoggingService(options =>
{
    options.MinimumLevel = LogLevel.Information;
    options.EnableConsoleLogging = true;
    options.EnableFileLogging = true;
    options.FilePath = "logs/app.log";
    options.FileRotationInterval = TimeSpan.FromDays(1);
    options.MaxFileSize = 10 * 1024 * 1024; // 10MB
    options.RetainedFileCount = 30;
    options.EnableStructuredLogging = true;
    options.EnableScopeLogging = true;
    options.EnablePerformanceLogging = true;
    options.PerformanceThreshold = TimeSpan.FromSeconds(1);
});
```

## Example Usage Scenarios

### 1. API Request Logging

```csharp
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public LoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrGenerateCorrelationId(context);

        using (_loggingService.WithCorrelationId(correlationId))
        using (_loggingService.BeginScope("HTTP {Method} {Path}", context.Request.Method, context.Request.Path))
        using (var timer = _loggingService.TimeOperation("Request processing"))
        {
            try
            {
                _loggingService.LogInformation(
                    "Request started: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path
                );

                await _next(context);

                _loggingService.LogInformation(
                    "Request completed: {StatusCode}",
                    context.Response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _loggingService.LogError(
                    ex,
                    "Request failed: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path
                );
                throw;
            }
        }
    }
}
```

### 2. Background Job Logging

```csharp
public class DataProcessingJob
{
    private readonly ILoggingService _loggingService;
    private readonly IDataProcessor _dataProcessor;

    public DataProcessingJob(ILoggingService loggingService, IDataProcessor dataProcessor)
    {
        _loggingService = loggingService;
        _dataProcessor = dataProcessor;
    }

    public async Task ExecuteAsync()
    {
        using (_loggingService.BeginScope("Data processing job"))
        using (var timer = _loggingService.TimeOperation("Job execution"))
        {
            try
            {
                _loggingService.LogInformation("Starting data processing job");

                var data = await LoadData();
                _loggingService.LogInformation("Loaded {Count} records", data.Count);

                var results = await ProcessData(data);
                _loggingService.LogInformation("Processed {Count} records", results.Count);

                await SaveResults(results);
                _loggingService.LogInformation("Saved processing results");
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Data processing job failed");
                throw;
            }
        }
    }
}
```

## Best Practices

1. **Log Levels**
   - Use appropriate log levels
   - Include context information
   - Use structured logging
   - Avoid sensitive data logging

2. **Performance**
   - Use sampling for high-volume logs
   - Implement log buffering
   - Configure appropriate retention
   - Monitor logging performance

3. **Error Handling**
   - Include stack traces
   - Add correlation IDs
   - Log operation context
   - Handle logging failures

4. **Security**
   - Sanitize logged data
   - Implement log access control
   - Encrypt sensitive logs
   - Follow compliance requirements

5. **Maintenance**
   - Implement log rotation
   - Configure log retention
   - Monitor log storage
   - Archive old logs

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 