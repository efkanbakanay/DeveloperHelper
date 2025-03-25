using DeveloperHelper.Cache;
using Xunit;
using FluentAssertions;

namespace DeveloperHelper.Testing.Cache;

public class CacheHelperTests
{
    [Fact]
    public void Set_ShouldStoreValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";

        // Act
        CacheHelper.Set(key, value);

        // Assert
        CacheHelper.Exists(key).Should().BeTrue();
    }

    [Fact]
    public void Get_WithExistingValue_ShouldReturnValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        CacheHelper.Set(key, value);

        // Act
        var result = CacheHelper.Get<string>(key);

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void Get_WithNonExistingValue_ShouldReturnDefault()
    {
        // Arrange
        var key = "non-existing-key";

        // Act
        var result = CacheHelper.Get<string>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetOrSet_WithNonExistingValue_ShouldSetAndReturnValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";

        // Act
        var result = CacheHelper.GetOrSet(key, () => value);

        // Assert
        result.Should().Be(value);
        CacheHelper.Exists(key).Should().BeTrue();
    }

    [Fact]
    public void GetOrSet_WithExistingValue_ShouldReturnExistingValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        CacheHelper.Set(key, value);

        // Act
        var result = CacheHelper.GetOrSet(key, () => "new-value");

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public async Task GetOrSetAsync_WithNonExistingValue_ShouldSetAndReturnValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";

        // Act
        var result = await CacheHelper.GetOrSetAsync(key, () => Task.FromResult(value));

        // Assert
        result.Should().Be(value);
        CacheHelper.Exists(key).Should().BeTrue();
    }

    [Fact]
    public async Task GetOrSetAsync_WithExistingValue_ShouldReturnExistingValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        CacheHelper.Set(key, value);

        // Act
        var result = await CacheHelper.GetOrSetAsync(key, () => Task.FromResult("new-value"));

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void Remove_ShouldRemoveValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        CacheHelper.Set(key, value);

        // Act
        CacheHelper.Remove(key);

        // Assert
        CacheHelper.Exists(key).Should().BeFalse();
    }

    [Fact]
    public void Clear_ShouldRemoveAllValues()
    {
        // Arrange
        CacheHelper.Set("key1", "value1");
        CacheHelper.Set("key2", "value2");

        // Act
        CacheHelper.Clear();

        // Assert
        CacheHelper.Exists("key1").Should().BeFalse();
        CacheHelper.Exists("key2").Should().BeFalse();
    }

    [Fact]
    public void Exists_WithExistingValue_ShouldReturnTrue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        CacheHelper.Set(key, value);

        // Act
        var result = CacheHelper.Exists(key);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Exists_WithNonExistingValue_ShouldReturnFalse()
    {
        // Arrange
        var key = "non-existing-key";

        // Act
        var result = CacheHelper.Exists(key);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Update_WithExistingValue_ShouldUpdateValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        var newValue = "new-value";
        CacheHelper.Set(key, value);

        // Act
        var result = CacheHelper.Update(key, newValue);

        // Assert
        result.Should().BeTrue();
        CacheHelper.Get<string>(key).Should().Be(newValue);
    }

    [Fact]
    public void Update_WithNonExistingValue_ShouldReturnFalse()
    {
        // Arrange
        var key = "non-existing-key";
        var value = "test-value";

        // Act
        var result = CacheHelper.Update(key, value);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SetOrUpdate_ShouldSetOrUpdateValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";

        // Act
        CacheHelper.SetOrUpdate(key, value);

        // Assert
        CacheHelper.Get<string>(key).Should().Be(value);
    }
} 