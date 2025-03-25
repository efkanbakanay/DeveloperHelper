using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Ganss.Xss;
using DeveloperHelper.Logging;

namespace DeveloperHelper.Security;

/// <summary>
/// Security functionality for DeveloperHelper library
/// </summary>
public static class SecurityHelper
{
    /// <summary>
    /// Hashes a password using PBKDF2
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>The hashed password</returns>
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    /// <param name="password">The password to verify</param>
    /// <param name="hashedPassword">The hashed password</param>
    /// <returns>True if the password matches, false otherwise</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hash == hashed;
    }

    /// <summary>
    /// Generates a JWT token
    /// </summary>
    /// <param name="user">The user object to include in the token</param>
    /// <param name="secretKey">The secret key to sign the token</param>
    /// <param name="expiresIn">The time until the token expires</param>
    /// <returns>The JWT token</returns>
    public static string GenerateJwtToken(object user, string secretKey, TimeSpan? expiresIn = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.ToString() ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "DeveloperHelper",
            audience: "DeveloperHelper",
            claims: claims,
            expires: DateTime.UtcNow.Add(expiresIn ?? TimeSpan.FromHours(1)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <param name="secretKey">The secret key used to sign the token</param>
    /// <returns>The claims in the token if valid, null otherwise</returns>
    public static ClaimsPrincipal? ValidateJwtToken(string token, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "DeveloperHelper",
                ValidateAudience = true,
                ValidAudience = "DeveloperHelper",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Sanitizes HTML content to prevent XSS attacks
    /// </summary>
    /// <param name="html">The HTML content to sanitize</param>
    /// <returns>The sanitized HTML content</returns>
    public static string SanitizeHtml(string html)
    {
        var sanitizer = new HtmlSanitizer();
        return sanitizer.Sanitize(html);
    }

    /// <summary>
    /// Sanitizes a string to prevent SQL injection
    /// </summary>
    /// <param name="input">The string to sanitize</param>
    /// <returns>The sanitized string</returns>
    public static string SanitizeSql(string input)
    {
        return input.Replace("'", "''")
                   .Replace("--", "")
                   .Replace(";", "")
                   .Replace("/*", "")
                   .Replace("*/", "");
    }

    /// <summary>
    /// Encrypts a string using AES encryption
    /// </summary>
    /// <param name="plainText">The text to encrypt</param>
    /// <param name="key">The encryption key</param>
    /// <returns>The encrypted text</returns>
    public static string Encrypt(string plainText, string key)
    {
        try
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                // Derive key using PBKDF2
                using var deriveBytes = new Rfc2898DeriveBytes(key, iv, 10000, HashAlgorithmName.SHA512);
                aes.Key = deriveBytes.GetBytes(32);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }

            return Convert.ToBase64String(array);
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to encrypt text: {ex.Message}");
            throw new SecurityException("Encryption failed", ex);
        }
    }

    /// <summary>
    /// Decrypts a string using AES encryption
    /// </summary>
    /// <param name="cipherText">The text to decrypt</param>
    /// <param name="key">The encryption key</param>
    /// <returns>The decrypted text</returns>
    public static string Decrypt(string cipherText, string key)
    {
        try
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                // Derive key using PBKDF2
                using var deriveBytes = new Rfc2898DeriveBytes(key, iv, 10000, HashAlgorithmName.SHA512);
                aes.Key = deriveBytes.GetBytes(32);
                aes.IV = iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.LogError(ex, $"Failed to decrypt text: {ex.Message}");
            throw new SecurityException("Decryption failed", ex);
        }
    }
} 