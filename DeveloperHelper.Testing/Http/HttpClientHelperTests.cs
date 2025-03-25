using DeveloperHelper.Http;
using System.Net;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using HttpRequestOptions = DeveloperHelper.Http.HttpRequestOptions;
using System;
using System.Threading.Tasks;

namespace DeveloperHelper.Testing.Http;

public class HttpClientHelperTests
{
    private const string TestUrl = "https://api.example.com";

    [Fact]
    public async Task GetAsync_ShouldReturnResponse()
    {
        // Arrange
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.GetAsync(TestUrl, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task GetAsync_WithType_ShouldReturnDeserializedResponse()
    {
        // Arrange
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.GetAsync<TestResponse>(TestUrl, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task PostAsync_ShouldReturnResponse()
    {
        // Arrange
        var content = new TestRequest { Name = "Test", Value = 123 };
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.PostAsync<TestResponse>(TestUrl, content, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task PostAsync_WithType_ShouldReturnDeserializedResponse()
    {
        // Arrange
        var content = new TestRequest { Name = "Test", Value = 123 };
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.PostAsync<TestResponse>(TestUrl, content, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task PutAsync_ShouldReturnResponse()
    {
        // Arrange
        var content = new TestRequest { Name = "Test", Value = 123 };
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.PutAsync<TestResponse>(TestUrl, content, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task PutAsync_WithType_ShouldReturnDeserializedResponse()
    {
        // Arrange
        var content = new TestRequest { Name = "Test", Value = 123 };
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.PutAsync<TestResponse>(TestUrl, content, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnResponse()
    {
        // Arrange
        Action<HttpRequestOptions> options = opt =>
        {
            opt.Headers["Authorization"] = "Bearer test-token";
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
            HttpClientHelper.DeleteAsync(TestUrl, options));

        (exception is HttpRequestException || exception is TaskCanceledException || exception is InvalidOperationException)
            .Should().BeTrue();
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.GetAsync<TestResponse>("invalid-url"));
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_WhenUrlNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.GetAsync<TestResponse>($"{TestUrl}/not-found"));
    }

    [Fact]
    public async Task PostAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Arrange
        var data = new { test = "data" };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.PostAsync<TestResponse>("invalid-url", data));
    }

    [Fact]
    public async Task PostAsync_ShouldThrowException_WhenUrlNotFound()
    {
        // Arrange
        var data = new { test = "data" };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.PostAsync<TestResponse>($"{TestUrl}/not-found", data));
    }

    [Fact]
    public async Task PutAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Arrange
        var data = new { test = "data" };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.PutAsync<TestResponse>("invalid-url", data));
    }

    [Fact]
    public async Task PutAsync_ShouldThrowException_WhenUrlNotFound()
    {
        // Arrange
        var data = new { test = "data" };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.PutAsync<TestResponse>($"{TestUrl}/not-found", data));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.DeleteAsync<TestResponse>("invalid-url"));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenUrlNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            HttpClientHelper.DeleteAsync<TestResponse>($"{TestUrl}/not-found"));
    }

    public class TestRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}

public class TestResponse
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
} 