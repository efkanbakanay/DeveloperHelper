using Serilog;
using Serilog.Events;

namespace DeveloperHelper.Logging;

/// <summary>
/// Logging functionality for DeveloperHelper library
/// </summary>
public static class LoggerHelper
{
    private static ILogger? _logger;
    private static readonly object _lock = new();

    /// <summary>
    /// Configures the logger with the specified configuration
    /// </summary>
    /// <param name="configure">The configuration action</param>
    public static void Configure(Action<LoggerConfiguration> configure)
    {
        if (_logger != null) return;

        lock (_lock)
        {
            if (_logger != null) return;

            var configuration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File("logs/developer-helper-.txt", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31);

            configure(configuration);

            _logger = configuration.CreateLogger();
        }
    }

    /// <summary>
    /// Ensures the logger is configured
    /// </summary>
    private static void EnsureConfigured()
    {
        if (_logger == null)
        {
            Configure(_ => { });
        }
    }

    /// <summary>
    /// Logs a debug message
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void LogDebug(string message)
    {
        EnsureConfigured();
        _logger?.Debug(message);
    }

    /// <summary>
    /// Logs an information message
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void LogInformation(string message)
    {
        EnsureConfigured();
        _logger?.Information(message);
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void LogWarning(string message)
    {
        EnsureConfigured();
        _logger?.Warning(message);
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void LogError(string message)
    {
        EnsureConfigured();
        _logger?.Error(message);
    }

    /// <summary>
    /// Logs an error message with exception details
    /// </summary>
    /// <param name="exception">The exception to log</param>
    /// <param name="message">The message to log</param>
    public static void LogError(Exception exception, string message)
    {
        EnsureConfigured();
        _logger?.Error(exception, message);
    }

    /// <summary>
    /// Logs a critical message
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void LogCritical(string message)
    {
        EnsureConfigured();
        _logger?.Fatal(message);
    }

    /// <summary>
    /// Logs a message at the specified level
    /// </summary>
    /// <param name="level">The log level</param>
    /// <param name="message">The message to log</param>
    public static void Log(LogEventLevel level, string message)
    {
        EnsureConfigured();
        _logger?.Write(level, message);
    }

    /// <summary>
    /// Logs a message at the specified level with exception details
    /// </summary>
    /// <param name="level">The log level</param>
    /// <param name="exception">The exception to log</param>
    /// <param name="message">The message to log</param>
    public static void Log(LogEventLevel level, Exception exception, string message)
    {
        EnsureConfigured();
        _logger?.Write(level, exception, message);
    }
} 