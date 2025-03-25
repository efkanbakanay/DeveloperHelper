using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DeveloperHelper.Core;

/// <summary>
/// Core functionality for DeveloperHelper library
/// </summary>
public static class DeveloperHelper
{
    /// <summary>
    /// Adds DeveloperHelper services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddDeveloperHelper(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
} 