using Xunit;
using FluentAssertions;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace DeveloperHelper.Testing.Logging;

public class LoggerHelperTests
{
    [Fact]
    public async Task Configure_ShouldConfigureLogger()
    {
        // Act
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.Configure(config => config.MinimumLevel.Debug()));

        // Assert
        Log.Logger.Should().NotBeNull();
    }

    [Fact]
    public async Task LogDebug_ShouldLogMessage()
    {
        // Arrange
        var message = "Debug message";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogDebug(message));
    }

    [Fact]
    public async Task LogInformation_ShouldLogMessage()
    {
        // Arrange
        var message = "Information message";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogInformation(message));
    }

    [Fact]
    public async Task LogWarning_ShouldLogMessage()
    {
        // Arrange
        var message = "Warning message";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogWarning(message));
    }

    [Fact]
    public async Task LogError_ShouldLogMessage()
    {
        // Arrange
        var message = "Error message";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogError(message));
    }

    [Fact]
    public async Task LogError_WithException_ShouldLogMessage()
    {
        // Arrange
        var message = "Error message with exception";
        var exception = new Exception("Test exception");

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogError(exception, message));
    }

    [Fact]
    public async Task LogCritical_ShouldLogMessage()
    {
        // Arrange
        var message = "Critical message";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.LogCritical(message));
    }

    [Fact]
    public async Task Log_WithLevel_ShouldLogMessage()
    {
        // Arrange
        var message = "Log message with level";

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.Log(LogEventLevel.Information, message));
    }

    [Fact]
    public async Task Log_WithLevelAndException_ShouldLogMessage()
    {
        // Arrange
        var message = "Log message with level and exception";
        var exception = new Exception("Test exception");

        // Act & Assert
        await Task.Run(() => DeveloperHelper.Logging.LoggerHelper.Log(LogEventLevel.Error, exception, message));
    }
} 