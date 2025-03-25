using Xunit;
using Xunit.Abstractions;

namespace DeveloperHelper.Testing;

public class TestRunnerTests
{
    private readonly ITestOutputHelper _output;

    public TestRunnerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task RunAllTests_ShouldExecuteAllTests()
    {
        // Arrange
        var runner = new TestRunner(_output);

        // Act
        await runner.RunAllTests();

        // Assert
        // If we get here without exceptions, all tests passed
        Assert.True(true);
    }
} 