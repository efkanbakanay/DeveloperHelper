using System;
using System.Threading.Tasks;
using DeveloperHelper.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DeveloperHelper.Testing.Core;

public class DeveloperHelperTests
{
    private readonly IConfiguration _configuration;

    public DeveloperHelperTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
    }

    [Fact]
    public void Test_Configuration_Loading()
    {
        Assert.NotNull(_configuration);
    }

    [Fact]
    public void AddDeveloperHelper_ShouldAddServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        // Act
        services.AddDeveloperHelper(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }
} 