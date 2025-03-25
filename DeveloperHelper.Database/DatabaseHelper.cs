using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Linq;
using System.Security;
using System.Data;
using System.Threading.Tasks;
using DeveloperHelper.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DeveloperHelper.Database;

/// <summary>
/// Custom exception for database operations
/// </summary>
public class DatabaseException : Exception
{
    public DatabaseException(string message) : base(message) { }
    public DatabaseException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public interface IDatabaseHelper
{
    Task<SqlConnection> BeginTransactionAsync();
    Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object? parameters = null);
    Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, object? parameters = null);
    Task<int> ExecuteCommandAsync(string command, object? parameters = null);
    Task<int> ExecuteTransactionAsync(IEnumerable<(string command, object? parameters)> commands);
}

/// <summary>
/// Database functionality for DeveloperHelper library
/// </summary>
public class DatabaseHelper : IDatabaseHelper
{
    private readonly string _connectionString;
    private readonly ILogger _logger;

    public DatabaseHelper(string connectionString, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty");
        }

        try
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
            {
                throw new ArgumentException("Database name is required in connection string", nameof(connectionString));
            }
            _connectionString = builder.ConnectionString;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid connection string format", nameof(connectionString), ex);
        }

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executes a query and returns the results
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="sql">The SQL query</param>
    /// <param name="parameters">The query parameters</param>
    /// <returns>The query results</returns>
    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object? parameters = null)
    {
        try
        {
            ValidateQuery(sql);
            if (parameters != null)
                ValidateParameters(parameters);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, parameters);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing query: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute database query", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error executing query: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute database query", ex);
        }
    }

    /// <summary>
    /// Executes a stored procedure and returns the results
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="procedureName">The name of the stored procedure</param>
    /// <param name="parameters">The procedure parameters</param>
    /// <returns>The procedure results</returns>
    public async Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, object? parameters = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("Procedure name cannot be null or empty", nameof(procedureName));

            if (parameters != null)
                ValidateParameters(parameters);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var result = await connection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result ?? throw new DatabaseException($"No result returned from stored procedure '{procedureName}'");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing stored procedure: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute stored procedure", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error executing stored procedure: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute stored procedure", ex);
        }
    }

    /// <summary>
    /// Executes a command and returns the number of rows affected
    /// </summary>
    /// <param name="sql">The SQL command</param>
    /// <param name="parameters">The command parameters</param>
    /// <returns>The number of rows affected</returns>
    public async Task<int> ExecuteCommandAsync(string sql, object? parameters = null)
    {
        try
        {
            ValidateQuery(sql);
            if (parameters != null)
                ValidateParameters(parameters);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, parameters);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing command: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute database command", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error executing command: {Message}", ex.Message);
            throw new DatabaseException("Failed to execute database command", ex);
        }
    }

    /// <summary>
    /// Executes multiple SQL commands in a transaction
    /// </summary>
    /// <param name="commands">The SQL commands to execute</param>
    /// <returns>The number of rows affected</returns>
    public async Task<int> ExecuteTransactionAsync(IEnumerable<(string command, object? parameters)> commands)
    {
        if (commands == null || !commands.Any())
        {
            throw new ArgumentException("Commands collection cannot be null or empty", nameof(commands));
        }

        // Validate all commands before attempting to connect
        foreach (var (command, parameters) in commands)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Command cannot be null or empty");
            }

            if (IsDangerousString(command))
            {
                throw new SecurityException("Potentially dangerous SQL command detected");
            }

            if (parameters != null)
            {
                ValidateParameters(parameters);
            }
        }

        using var connection = await BeginTransactionAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            var totalRowsAffected = 0;
            foreach (var (command, parameters) in commands)
            {
                totalRowsAffected += await connection.ExecuteAsync(command, parameters, transaction);
            }

            transaction.Commit();
            return totalRowsAffected;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing transaction: {Message}", ex.Message);
            transaction.Rollback();
            throw new DatabaseException("Failed to execute database transaction", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error executing transaction: {Message}", ex.Message);
            transaction.Rollback();
            throw new DatabaseException("Failed to execute database transaction", ex);
        }
    }

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    /// <returns>The transaction</returns>
    public async Task<SqlConnection> BeginTransactionAsync()
    {
        try
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Failed to open database connection: {Message}", ex.Message);
            throw new DatabaseException("Failed to open database connection", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while opening database connection: {Message}", ex.Message);
            throw new DatabaseException("Failed to open database connection", ex);
        }
    }

    /// <summary>
    /// Executes a SQL command and returns the first result
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="command">The SQL command</param>
    /// <param name="parameters">The parameters for the command</param>
    /// <returns>The first result of the command</returns>
    public static async Task<T?> ExecuteScalarAsync<T>(
        string connectionString,
        string command,
        object? parameters = null)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.ExecuteScalarAsync<T>(command, parameters);
    }

    /// <summary>
    /// Creates a DbContext for Entity Framework Core
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext</typeparam>
    /// <returns>The DbContext</returns>
    public TContext CreateDbContext<TContext>() where TContext : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
    }

    private static void ValidateQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be null or empty", nameof(query));

        // Basic SQL injection check
        var dangerousKeywords = new[] { "--", ";", "/*", "*/", "xp_", "exec", "sp_", "sys" };
        if (dangerousKeywords.Any(keyword => query.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            throw new SecurityException("Potentially dangerous SQL detected");
    }

    private static void ValidateParameters(object parameters)
    {
        if (parameters == null) return;

        var properties = parameters.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(parameters);
            if (value is string strValue && IsDangerousString(strValue))
                throw new SecurityException($"Potentially dangerous parameter value detected in {property.Name}");
        }
    }

    private static bool IsDangerousString(string value)
    {
        var dangerousPatterns = new[]
        {
            @"--",
            @";",
            @"/*",
            @"*/",
            @"xp_",
            @"exec",
            @"sp_",
            @"sys",
            @"'",
            @"""",
            @"\",
            @"%",
            @"_"
        };

        return dangerousPatterns.Any(pattern => value.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
} 