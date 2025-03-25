# DeveloperHelper.Database

This module provides a robust and efficient database access layer for your .NET applications. It offers a simplified interface for database operations with support for transactions, connection management, and error handling.

## Features

- Simplified database operations
- Transaction support
- Connection pooling
- Error handling and logging
- SQL injection prevention
- Parameter validation
- Connection retry policies
- Circuit breaker pattern
- Performance monitoring
- Query logging

## Installation

```bash
dotnet add package DeveloperHelper.Database
```

## Usage

### Basic Configuration

```csharp
using DeveloperHelper.Database;

// Configure database service
services.AddDatabaseService(options =>
{
    options.ConnectionString = "Server=localhost;Database=MyDb;User Id=sa;Password=YourPassword;";
    options.MaxRetries = 3;
    options.CommandTimeout = 30; // seconds
    options.EnableQueryLogging = true;
    options.EnablePerformanceMonitoring = true;
});
```

### Basic Database Operations

```csharp
public class UserService
{
    private readonly IDatabaseService _databaseService;

    public UserService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var query = "SELECT * FROM Users WHERE Id = @Id";
        var parameters = new { Id = userId };
        
        return await _databaseService.QueryFirstOrDefaultAsync<User>(query, parameters);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        var query = @"
            INSERT INTO Users (Name, Email, CreatedAt)
            VALUES (@Name, @Email, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            
        var parameters = new
        {
            user.Name,
            user.Email,
            CreatedAt = DateTime.UtcNow
        };
        
        return await _databaseService.QuerySingleAsync<int>(query, parameters);
    }
}
```

### Transaction Support

```csharp
public class OrderService
{
    private readonly IDatabaseService _databaseService;

    public OrderService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using (var transaction = await _databaseService.BeginTransactionAsync())
        {
            try
            {
                // Create order
                var orderId = await _databaseService.QuerySingleAsync<int>(
                    "INSERT INTO Orders (CustomerId, Total) VALUES (@CustomerId, @Total); SELECT CAST(SCOPE_IDENTITY() as int)",
                    new { order.CustomerId, order.Total }
                );

                // Create order items
                foreach (var item in order.Items)
                {
                    await _databaseService.ExecuteAsync(
                        "INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)",
                        new { OrderId = orderId, item.ProductId, item.Quantity, item.Price }
                    );
                }

                // Update inventory
                foreach (var item in order.Items)
                {
                    await _databaseService.ExecuteAsync(
                        "UPDATE Products SET Stock = Stock - @Quantity WHERE Id = @ProductId",
                        new { item.ProductId, item.Quantity }
                    );
                }

                await transaction.CommitAsync();
                return await GetOrderAsync(orderId);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
```

## Features and Methods

### IDatabaseService

- `QueryAsync<T>(string sql, object parameters = null)`: Execute query and return results
- `QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)`: Execute query and return first result
- `QuerySingleAsync<T>(string sql, object parameters = null)`: Execute query and return single result
- `ExecuteAsync(string sql, object parameters = null)`: Execute command and return affected rows
- `BeginTransactionAsync()`: Begin a new transaction
- `GetConnectionAsync()`: Get a database connection
- `ExecuteStoredProcedureAsync<T>(string procedureName, object parameters = null)`: Execute stored procedure
- `BulkInsertAsync<T>(IEnumerable<T> items, string tableName)`: Bulk insert items
- `GetQueryPlanAsync(string sql, object parameters = null)`: Get query execution plan

## Configuration

### Database Service Configuration

```csharp
services.AddDatabaseService(options =>
{
    options.ConnectionString = "Server=localhost;Database=MyDb;User Id=sa;Password=YourPassword;";
    options.MaxRetries = 3;
    options.CommandTimeout = 30;
    options.EnableQueryLogging = true;
    options.EnablePerformanceMonitoring = true;
    options.ConnectionPoolSize = 100;
    options.EnableCircuitBreaker = true;
    options.CircuitBreakerThreshold = 5;
    options.CircuitBreakerResetTimeout = TimeSpan.FromSeconds(30);
    options.EnableQueryPlanCaching = true;
    options.QueryPlanCacheSize = 1000;
});
```

## Example Usage Scenarios

### 1. Complex Query with Joins

```csharp
public class OrderRepository
{
    private readonly IDatabaseService _databaseService;

    public OrderRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<IEnumerable<OrderDetails>> GetOrderDetailsAsync(int customerId)
    {
        var query = @"
            SELECT o.Id, o.OrderDate, o.Total,
                   c.Name as CustomerName, c.Email as CustomerEmail,
                   p.Name as ProductName, p.Price as ProductPrice,
                   oi.Quantity
            FROM Orders o
            JOIN Customers c ON o.CustomerId = c.Id
            JOIN OrderItems oi ON o.Id = oi.OrderId
            JOIN Products p ON oi.ProductId = p.Id
            WHERE o.CustomerId = @CustomerId
            ORDER BY o.OrderDate DESC";

        return await _databaseService.QueryAsync<OrderDetails>(query, new { CustomerId = customerId });
    }
}
```

### 2. Bulk Operations

```csharp
public class ProductRepository
{
    private readonly IDatabaseService _databaseService;

    public ProductRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task UpdateProductPricesAsync(IEnumerable<ProductPrice> prices)
    {
        using (var transaction = await _databaseService.BeginTransactionAsync())
        {
            try
            {
                foreach (var batch in prices.Chunk(1000))
                {
                    await _databaseService.ExecuteAsync(
                        "UPDATE Products SET Price = @Price WHERE Id = @Id",
                        batch
                    );
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
```

## Best Practices

1. **Query Performance**
   - Use parameterized queries
   - Index frequently queried columns
   - Optimize query plans
   - Use appropriate data types

2. **Connection Management**
   - Use connection pooling
   - Implement retry policies
   - Handle connection failures
   - Monitor connection usage

3. **Transaction Management**
   - Keep transactions short
   - Handle deadlocks
   - Implement proper rollback
   - Use appropriate isolation levels

4. **Security**
   - Prevent SQL injection
   - Validate parameters
   - Use least privilege
   - Encrypt sensitive data

5. **Monitoring**
   - Log slow queries
   - Monitor performance
   - Track errors
   - Set up alerts

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 