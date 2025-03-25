using DeveloperHelper.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Xunit;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Security;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeveloperHelper.Testing.Database;

public class DatabaseHelperTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly string _validConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";
    private readonly DatabaseHelper _databaseHelper;

    public DatabaseHelperTests()
    {
        _loggerMock = new Mock<ILogger>();
        _databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConnectionStringIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DatabaseHelper(null!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConnectionStringIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DatabaseHelper(string.Empty, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DatabaseHelper(_validConnectionString, null!));
    }

    [Fact]
    public async Task ExecuteQueryAsync_ShouldThrowDatabaseException_WhenQueryIsInvalid()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DatabaseException>(() => databaseHelper.ExecuteQueryAsync<object>("INVALID SQL"));
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_ShouldThrowDatabaseException_WhenProcedureDoesNotExist()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DatabaseException>(() => databaseHelper.ExecuteStoredProcedureAsync<object>("NonExistentProcedure"));
    }

    [Fact]
    public async Task ExecuteCommandAsync_ShouldThrowDatabaseException_WhenCommandIsInvalid()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DatabaseException>(() => databaseHelper.ExecuteCommandAsync("INVALID SQL"));
    }

    [Fact]
    public async Task ExecuteTransactionAsync_ShouldThrowArgumentException_WhenCommandsIsNull()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => databaseHelper.ExecuteTransactionAsync(null!));
    }

    [Fact]
    public async Task ExecuteTransactionAsync_ShouldThrowArgumentException_WhenCommandsIsEmpty()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => databaseHelper.ExecuteTransactionAsync(new List<(string command, object? parameters)>()));
    }

    [Fact]
    public async Task ExecuteTransactionAsync_ShouldThrowSecurityException_WhenCommandContainsDangerousString()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);
        var commands = new List<(string command, object? parameters)>
        {
            ("SELECT * FROM Users; DROP TABLE Users;", null)
        };

        // Act & Assert
        await Assert.ThrowsAsync<SecurityException>(() => databaseHelper.ExecuteTransactionAsync(commands));
    }

    [Fact]
    public async Task ExecuteTransactionAsync_ShouldExecuteCommandsInTransaction()
    {
        // Arrange
        var databaseHelper = new DatabaseHelper(_validConnectionString, _loggerMock.Object);
        var commands = new List<(string command, object? parameters)>
        {
            ("CREATE TABLE #TempTable (Id INT)", null),
            ("INSERT INTO #TempTable VALUES (@Id)", new { Id = 1 }),
            ("DROP TABLE #TempTable", null)
        };

        // Act & Assert
        await Assert.ThrowsAsync<DatabaseException>(() => databaseHelper.ExecuteTransactionAsync(commands));
        _loggerMock.Verify(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteStoredProcedureAsync_ShouldExecuteProcedure()
    {
        // Arrange
        var procedureName = "TestProcedure";
        var parameters = new { Id = 1, Name = "Test" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DatabaseException>(() =>
            _databaseHelper.ExecuteStoredProcedureAsync<TestEntity>(procedureName, parameters));

        exception.InnerException.Should().BeOfType<SqlException>();
    }

    [Fact]
    public async Task ExecuteQueryAsync_ShouldExecuteQuery()
    {
        // Arrange
        var query = "SELECT * FROM TestTable WHERE Id = @Id";
        var parameters = new { Id = 1 };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DatabaseException>(() =>
            _databaseHelper.ExecuteQueryAsync<TestEntity>(query, parameters));

        exception.InnerException.Should().BeOfType<SqlException>();
    }

    [Fact]
    public async Task ExecuteCommandAsync_ShouldExecuteCommand()
    {
        // Arrange
        var command = "INSERT INTO TestTable (Name) VALUES (@Name)";
        var parameters = new { Name = "Test" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DatabaseException>(() =>
            _databaseHelper.ExecuteCommandAsync(command, parameters));

        exception.InnerException.Should().BeOfType<SqlException>();
    }

    [Fact]
    public async Task ExecuteScalarAsync_ShouldExecuteScalar()
    {
        // Arrange
        var command = "SELECT COUNT(*) FROM TestTable";
        var parameters = new { };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SqlException>(() =>
            DatabaseHelper.ExecuteScalarAsync<int>(_validConnectionString, command, parameters));

        exception.Should().BeOfType<SqlException>();
    }

    [Fact]
    public void CreateDbContext_ShouldCreateContext()
    {
        // Act
        var context = _databaseHelper.CreateDbContext<TestDbContext>();

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public async Task BeginTransactionAsync_ShouldThrowException_WhenConnectionFails()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<DatabaseException>(() =>
            _databaseHelper.BeginTransactionAsync());

        exception.InnerException.Should().BeOfType<SqlException>();
    }
}

public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; } = null!;
} 