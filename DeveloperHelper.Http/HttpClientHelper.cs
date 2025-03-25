using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using DeveloperHelper.Logging;

namespace DeveloperHelper.Http;

/// <summary>
/// HTTP functionality for DeveloperHelper library
/// </summary>
public static class HttpClientHelper
{
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    static HttpClientHelper()
    {
        _httpClient.DefaultRequestHeaders.Add("X-Content-Type-Options", "nosniff");
        _httpClient.DefaultRequestHeaders.Add("X-Frame-Options", "DENY");
        _httpClient.DefaultRequestHeaders.Add("X-XSS-Protection", "1; mode=block");
        _httpClient.DefaultRequestHeaders.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        _httpClient.DefaultRequestHeaders.Add("Content-Security-Policy", "default-src 'self'");
    }

    private static readonly IAsyncPolicy<HttpResponseMessage> _circuitBreaker = Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .Or<TaskCanceledException>()
        .Or<InvalidOperationException>()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

    private static readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy = Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .Or<TaskCanceledException>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static readonly IAsyncPolicy<HttpResponseMessage> _policy = Policy.WrapAsync(_retryPolicy, _circuitBreaker);

    /// <summary>
    /// Sends a GET request and returns the response
    /// </summary>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="options">The request options</param>
    /// <returns>The response content</returns>
    public static async Task<string> GetAsync(string url, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            var requestOptions = new HttpRequestOptions();
            options?.Invoke(requestOptions);

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(request, requestOptions.Headers);

            var response = await ExecuteWithPoliciesAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send GET request: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends a GET request and returns the response deserialized to the specified type
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="options">The request options</param>
    /// <returns>The deserialized response</returns>
    public static async Task<T?> GetAsync<T>(string url, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            var requestOptions = new HttpRequestOptions();
            options?.Invoke(requestOptions);

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(request, requestOptions.Headers);

            using var response = await ExecuteWithPoliciesAsync(request);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send GET request: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends a POST request and returns the response
    /// </summary>
    /// <typeparam name="T">The type of the response</typeparam>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="data">The data to send</param>
    /// <param name="options">The request options</param>
    /// <returns>The response</returns>
    public static async Task<T?> PostAsync<T>(string url, object data, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            var jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var stringContent = new StringContent(jsonContent, Encoding.UTF8);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = stringContent
            };

            if (options != null)
            {
                var requestOptions = new HttpRequestOptions();
                options(requestOptions);
                
                foreach (var header in requestOptions.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using var response = await ExecuteWithPoliciesAsync(request);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send POST request: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends a PUT request and returns the response
    /// </summary>
    /// <typeparam name="T">The type of the response</typeparam>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="data">The data to send</param>
    /// <param name="options">The request options</param>
    /// <returns>The response</returns>
    public static async Task<T?> PutAsync<T>(string url, object data, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            var jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var stringContent = new StringContent(jsonContent, Encoding.UTF8);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = stringContent
            };

            if (options != null)
            {
                var requestOptions = new HttpRequestOptions();
                options(requestOptions);
                
                foreach (var header in requestOptions.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using var response = await ExecuteWithPoliciesAsync(request);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send PUT request: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends a DELETE request and returns the response
    /// </summary>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="options">The request options</param>
    /// <returns>The response content</returns>
    public static async Task<string> DeleteAsync(string url, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            var requestOptions = new HttpRequestOptions();
            options?.Invoke(requestOptions);

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            AddHeaders(request, requestOptions.Headers);

            var response = await ExecuteWithPoliciesAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send DELETE request: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends a DELETE request and returns the response deserialized to the specified type
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="url">The URL to send the request to</param>
    /// <param name="options">The request options</param>
    /// <returns>The deserialized response</returns>
    public static async Task<T?> DeleteAsync<T>(string url, Action<HttpRequestOptions>? options = null)
    {
        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new HttpRequestException("Invalid URL format");
            }

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            
            if (options != null)
            {
                var requestOptions = new HttpRequestOptions();
                options(requestOptions);
                
                foreach (var header in requestOptions.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using var response = await ExecuteWithPoliciesAsync(request);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to send DELETE request: {ex.Message}");
            throw;
        }
    }

    private static async Task<HttpResponseMessage> ExecuteWithPoliciesAsync(HttpRequestMessage request)
    {
        try
        {
            return await _policy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.SendAsync(request.Clone());
                response.EnsureSuccessStatusCode();
                return response;
            });
        }
        catch (BrokenCircuitException ex)
        {
            LoggerHelper.LogError(ex, "Circuit breaker is open - service unavailable");
            throw new HttpRequestException("Service is temporarily unavailable", ex);
        }
    }

    private static HttpRequestMessage Clone(this HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);
        
        if (request.Content != null)
        {
            var ms = new MemoryStream();
            request.Content.CopyToAsync(ms).Wait();
            ms.Position = 0;
            clone.Content = new StreamContent(ms);
            
            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.Add(header.Key, header.Value);
            }
        }
        
        foreach (var header in request.Headers)
        {
            clone.Headers.Add(header.Key, header.Value);
        }
        
        foreach (var option in request.Options)
        {
            clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
        }
        
        return clone;
    }

    private static async Task<HttpContent> CloneHttpContent(HttpContent content)
    {
        var ms = new MemoryStream();
        await content.CopyToAsync(ms);
        ms.Position = 0;

        var clone = new StreamContent(ms);
        foreach (var header in content.Headers)
        {
            clone.Headers.Add(header.Key, header.Value);
        }
        return clone;
    }

    private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }
}

/// <summary>
/// Options for HTTP requests
/// </summary>
public class HttpRequestOptions
{
    /// <summary>
    /// Gets or sets the headers to include in the request
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
} 