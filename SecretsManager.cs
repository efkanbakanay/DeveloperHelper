using System;
using Microsoft.Extensions.Configuration;

namespace DeveloperHelper
{
    /// <summary>
    /// API anahtarlarını ve diğer gizli bilgileri güvenli bir şekilde yönetmek için kullanılan sınıf.
    /// </summary>
    public class SecretsManager
    {
        private readonly IConfiguration _configuration;

        public SecretsManager()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SecretsManager>();
            _configuration = builder.Build();
        }

        /// <summary>
        /// NuGet API anahtarını güvenli bir şekilde alır.
        /// </summary>
        public string GetNuGetApiKey() => 
            _configuration["NuGet:ApiKey"] ?? 
            throw new InvalidOperationException("NuGet API anahtarı bulunamadı. Lütfen User Secrets veya appsettings.json dosyasını kontrol edin.");

        public string GetSecret(string key)
        {
            return _configuration[key];
        }

        public bool TryGetSecret(string key, out string value)
        {
            value = _configuration[key];
            return !string.IsNullOrEmpty(value);
        }
    }
} 