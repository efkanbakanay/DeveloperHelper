using FluentAssertions;
using Moq;
using Xunit;

namespace DeveloperHelper.Testing;

/// <summary>
/// Testing functionality for DeveloperHelper library
/// </summary>
public static class TestingHelper
{
    /// <summary>
    /// Creates a mock object of the specified type
    /// </summary>
    /// <typeparam name="T">The type to mock</typeparam>
    /// <returns>The mock object</returns>
    public static Mock<T> CreateMock<T>() where T : class
    {
        return new Mock<T>();
    }

    /// <summary>
    /// Creates a mock object of the specified type with the specified behavior
    /// </summary>
    /// <typeparam name="T">The type to mock</typeparam>
    /// <param name="behavior">The behavior of the mock</param>
    /// <returns>The mock object</returns>
    public static Mock<T> CreateMock<T>(MockBehavior behavior) where T : class
    {
        return new Mock<T>(behavior);
    }

    /// <summary>
    /// Creates a mock object of the specified type with the specified arguments
    /// </summary>
    /// <typeparam name="T">The type to mock</typeparam>
    /// <param name="args">The arguments for the constructor</param>
    /// <returns>The mock object</returns>
    public static Mock<T> CreateMock<T>(params object[] args) where T : class
    {
        return new Mock<T>(args);
    }

    /// <summary>
    /// Creates a mock object of the specified type with the specified behavior and arguments
    /// </summary>
    /// <typeparam name="T">The type to mock</typeparam>
    /// <param name="behavior">The behavior of the mock</param>
    /// <param name="args">The arguments for the constructor</param>
    /// <returns>The mock object</returns>
    public static Mock<T> CreateMock<T>(MockBehavior behavior, params object[] args) where T : class
    {
        return new Mock<T>(behavior, args);
    }

    /// <summary>
    /// Creates a test data generator for the specified type
    /// </summary>
    /// <typeparam name="T">The type to generate test data for</typeparam>
    /// <returns>The test data generator</returns>
    public static TestDataGenerator<T> CreateTestDataGenerator<T>()
    {
        return new TestDataGenerator<T>();
    }

    /// <summary>
    /// Creates a test data generator for the specified type with the specified options
    /// </summary>
    /// <typeparam name="T">The type to generate test data for</typeparam>
    /// <param name="options">The options for generating test data</param>
    /// <returns>The test data generator</returns>
    public static TestDataGenerator<T> CreateTestDataGenerator<T>(TestDataGeneratorOptions options)
    {
        return new TestDataGenerator<T>(options);
    }
}

/// <summary>
/// Options for generating test data
/// </summary>
public class TestDataGeneratorOptions
{
    /// <summary>
    /// Gets or sets whether to generate random values
    /// </summary>
    public bool UseRandomValues { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to generate unique values
    /// </summary>
    public bool UseUniqueValues { get; set; } = true;

    /// <summary>
    /// Gets or sets the seed for random number generation
    /// </summary>
    public int? RandomSeed { get; set; }
}

/// <summary>
/// A class for generating test data
/// </summary>
/// <typeparam name="T">The type to generate test data for</typeparam>
public class TestDataGenerator<T>
{
    private readonly TestDataGeneratorOptions _options;
    private readonly Random _random;

    /// <summary>
    /// Initializes a new instance of the TestDataGenerator class
    /// </summary>
    public TestDataGenerator()
        : this(new TestDataGeneratorOptions())
    {
    }

    /// <summary>
    /// Initializes a new instance of the TestDataGenerator class
    /// </summary>
    /// <param name="options">The options for generating test data</param>
    public TestDataGenerator(TestDataGeneratorOptions options)
    {
        _options = options;
        _random = _options.RandomSeed.HasValue
            ? new Random(_options.RandomSeed.Value)
            : new Random();
    }

    /// <summary>
    /// Generates a single test object
    /// </summary>
    /// <returns>The generated test object</returns>
    public T Generate()
    {
        return Activator.CreateInstance<T>();
    }

    /// <summary>
    /// Generates multiple test objects
    /// </summary>
    /// <param name="count">The number of test objects to generate</param>
    /// <returns>The generated test objects</returns>
    public IEnumerable<T> Generate(int count)
    {
        return Enumerable.Range(0, count).Select(_ => Generate());
    }
} 