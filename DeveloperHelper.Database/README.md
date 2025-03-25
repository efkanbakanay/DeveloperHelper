# DeveloperHelper.Database

This module is designed to simplify database operations in your .NET applications. It provides a robust and secure way to interact with SQL Server databases, including support for transactions, stored procedures, and parameterized queries.

## Features

- SQL Server database operations
- Transaction support
- Stored procedure execution
- Parameterized queries
- SQL injection protection
- Automatic connection management
- Error handling and logging
- Performance monitoring

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
    options.CommandTimeout = 30; // seconds
    options.EnableRetryPolicy = true;
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

    public async Task<User> GetUserAsync(int id)
    {
        var query = "SELECT * FROM Users WHERE Id = @Id";
        var parameters = new { Id = id };
        
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

    public async Task CreateOrderAsync(Order order, List<OrderItem> items)
    {
        await _databaseService.ExecuteTransactionAsync(async (transaction) =>
        {
            // Create order
            var orderId = await transaction.QuerySingleAsync<int>(
                "INSERT INTO Orders (CustomerId, TotalAmount) VALUES (@CustomerId, @TotalAmount); SELECT CAST(SCOPE_IDENTITY() as int)",
                new { order.CustomerId, order.TotalAmount }
            );

            // Create order items
            foreach (var item in items)
            {
                await transaction.ExecuteAsync(
                    "INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)",
                    new { OrderId = orderId, item.ProductId, item.Quantity, item.Price }
                );
            }
        });
    }
}
```

### Stored Procedure Execution

```csharp
public class ReportService
{
    private readonly IDatabaseService _databaseService;

    public ReportService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<SalesReport>> GetSalesReportAsync(DateTime startDate, DateTime endDate)
    {
        var parameters = new
        {
            StartDate = startDate,
            EndDate = endDate
        };

        return await _databaseService.ExecuteStoredProcedureAsync<SalesReport>(
            "sp_GetSalesReport",
            parameters
        );
    }
}
```

## Features and Methods

### IDatabaseService

- `QueryAsync<T>(string sql, object parameters = null)`: Execute a query and return results
- `QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)`: Execute a query and return first result or default
- `QuerySingleAsync<T>(string sql, object parameters = null)`: Execute a query and return single result
- `ExecuteAsync(string sql, object parameters = null)`: Execute a command and return rows affected
- `ExecuteStoredProcedureAsync<T>(string procedureName, object parameters = null)`: Execute a stored procedure
- `BeginTransactionAsync()`: Begin a new transaction
- `ExecuteTransactionAsync(Func<IDbTransaction, Task> action)`: Execute code within a transaction

## Configuration

### Database Service Configuration

```csharp
services.AddDatabaseService(options =>
{
    options.ConnectionString = "Server=localhost;Database=MyDb;User Id=sa;Password=YourPassword;";
    options.CommandTimeout = 30; // seconds
    options.EnableRetryPolicy = true;
    options.MaxRetries = 3;
    options.RetryInterval = TimeSpan.FromSeconds(1);
    options.EnablePerformanceMonitoring = true;
});
```

## Best Practices

1. **Connection Management**
   - Use connection pooling
   - Implement proper connection string management
   - Handle connection timeouts appropriately

2. **Query Optimization**
   - Use parameterized queries
   - Implement proper indexing
   - Monitor query performance

3. **Transaction Management**
   - Keep transactions as short as possible
   - Handle transaction rollbacks properly
   - Use appropriate isolation levels

4. **Error Handling**
   - Implement proper error logging
   - Use retry policies for transient failures
   - Handle deadlocks appropriately

5. **Security**
   - Use parameterized queries to prevent SQL injection
   - Implement proper access control
   - Encrypt sensitive data

## Example Usage Scenarios

### 1. Bulk Insert

```csharp
public class ProductService
{
    private readonly IDatabaseService _databaseService;

    public ProductService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task BulkInsertProductsAsync(List<Product> products)
    {
        var query = @"
            INSERT INTO Products (Name, Description, Price, CategoryId)
            VALUES (@Name, @Description, @Price, @CategoryId)";

        await _databaseService.ExecuteTransactionAsync(async (transaction) =>
        {
            foreach (var product in products)
            {
                await transaction.ExecuteAsync(query, product);
            }
        });
    }
}
```

### 2. Complex Query with Joins

```csharp
public class OrderService
{
    private readonly IDatabaseService _databaseService;

    public OrderService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<OrderDetails>> GetOrderDetailsAsync(int orderId)
    {
        var query = @"
            SELECT o.*, c.Name as CustomerName, 
                   p.Name as ProductName, oi.Quantity, oi.Price
            FROM Orders o
            JOIN Customers c ON o.CustomerId = c.Id
            JOIN OrderItems oi ON o.Id = oi.OrderId
            JOIN Products p ON oi.ProductId = p.Id
            WHERE o.Id = @OrderId";

        return await _databaseService.QueryAsync<OrderDetails>(query, new { OrderId = orderId });
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 