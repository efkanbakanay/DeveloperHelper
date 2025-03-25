using DeveloperHelper.Security;
using System.Security.Claims;
using Xunit;
using FluentAssertions;

namespace DeveloperHelper.Testing.Security;

public class SecurityHelperTests
{
    [Fact]
    public void HashPassword_ShouldHashPassword()
    {
        // Arrange
        var password = "test123";

        // Act
        var hashedPassword = SecurityHelper.HashPassword(password);

        // Assert
        hashedPassword.Should().NotBe(password);
        hashedPassword.Should().Contain(".");
    }

    [Fact]
    public void VerifyPassword_WithValidPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "test123";
        var hashedPassword = SecurityHelper.HashPassword(password);

        // Act
        var result = SecurityHelper.VerifyPassword(password, hashedPassword);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithInvalidPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "test123";
        var hashedPassword = SecurityHelper.HashPassword(password);
        var invalidPassword = "wrong123";

        // Act
        var result = SecurityHelper.VerifyPassword(invalidPassword, hashedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateValidToken()
    {
        // Arrange
        var user = "testuser";
        var secretKey = "testsecretkey123456789012345678901234567890";

        // Act
        var token = SecurityHelper.GenerateJwtToken(user, secretKey);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ValidateJwtToken_WithValidToken_ShouldReturnClaimsPrincipal()
    {
        // Arrange
        var user = "testuser";
        var secretKey = "testsecretkey123456789012345678901234567890";
        var token = SecurityHelper.GenerateJwtToken(user, secretKey);

        // Act
        var principal = SecurityHelper.ValidateJwtToken(token, secretKey);

        // Assert
        principal.Should().NotBeNull();
        principal?.Identity?.Name.Should().Be(user);
    }

    [Fact]
    public void ValidateJwtToken_WithInvalidToken_ShouldReturnNull()
    {
        // Arrange
        var secretKey = "testsecretkey123456789012345678901234567890";
        var invalidToken = "invalid.token.string";

        // Act
        var principal = SecurityHelper.ValidateJwtToken(invalidToken, secretKey);

        // Assert
        principal.Should().BeNull();
    }

    [Fact]
    public void SanitizeHtml_ShouldRemoveXssContent()
    {
        // Arrange
        var html = "<script>alert('xss')</script><p>Safe content</p>";

        // Act
        var sanitized = SecurityHelper.SanitizeHtml(html);

        // Assert
        sanitized.Should().NotContain("<script>");
        sanitized.Should().Contain("<p>Safe content</p>");
    }

    [Fact]
    public void SanitizeSql_ShouldRemoveSqlInjectionContent()
    {
        // Arrange
        var input = "'; DROP TABLE Users; --";

        // Act
        var sanitized = SecurityHelper.SanitizeSql(input);

        // Assert
        sanitized.Should().NotContain("';");
        sanitized.Should().NotContain("--");
    }

    [Fact]
    public void EncryptAndDecrypt_ShouldWorkCorrectly()
    {
        // Arrange
        var input = "test123";
        var key = "testkey123456789012345678901234567890";

        // Act
        var encrypted = SecurityHelper.Encrypt(input, key);
        var decrypted = SecurityHelper.Decrypt(encrypted, key);

        // Assert
        encrypted.Should().NotBe(input);
        decrypted.Should().Be(input);
    }
} 