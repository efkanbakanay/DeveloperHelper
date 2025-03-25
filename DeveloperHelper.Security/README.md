# DeveloperHelper Library

A comprehensive .NET library providing essential developer tools and utilities for common programming tasks.

## Table of Contents
- [Installation](#installation)
- [Modules](#modules)
  - [Core](#core)
  - [Security](#security)
  - [Logging](#logging)
  - [Cache](#cache)
  - [Database](#database)
  - [HTTP](#http)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package DeveloperHelper.Core
dotnet add package DeveloperHelper.Security
dotnet add package DeveloperHelper.Logging
dotnet add package DeveloperHelper.Cache
dotnet add package DeveloperHelper.Database
dotnet add package DeveloperHelper.Http
```

## Modules

### Core

The Core module provides essential utility functions for common programming tasks.

```csharp
// String Operations
string slug = "Hello World!".ToSlug(); // "hello-world"
string camelCase = "hello_world".ToCamelCase(); // "helloWorld"
string pascalCase = "hello_world".ToPascalCase(); // "HelloWorld"
string snakeCase = "HelloWorld".ToSnakeCase(); // "hello_world"

// Number Parsing
int? number = "123".ParseInt();
decimal? amount = "123.45".ParseDecimal();
DateTime? date = "2024-03-20".ParseDateTime();
bool? flag = "true".ParseBool();

// File Operations
FileHelper.CreateDirectoryIfNotExists("path/to/dir");
string content = FileHelper.ReadAllText("file.txt");
FileHelper.WriteAllText("file.txt", "content");
bool exists = FileHelper.FileExists("file.txt");
bool isDirectory = FileHelper.IsDirectory("path/to/dir");

// JSON Operations
var json = JsonHelper.Serialize(new { Name = "John", Age = 30 });
var obj = JsonHelper.Deserialize<User>(json);
var prettyJson = JsonHelper.PrettyPrint(json);

// XML Operations
var xml = XmlHelper.Serialize(new { Name = "John", Age = 30 });
var xmlObj = XmlHelper.Deserialize<User>(xml);
var prettyXml = XmlHelper.PrettyPrint(xml);

// CSV Operations
var csv = CsvHelper.Serialize(users);
var users = CsvHelper.Deserialize<User>(csv);

// Validation
bool isValidEmail = "john@example.com".IsValidEmail();
bool isValidUrl = "https://example.com".IsValidUrl();
bool isValidPhone = "+1234567890".IsValidPhone();
bool isValidIpAddress = "192.168.1.1".IsValidIpAddress();

// Formatting
string formattedNumber = 1234567.89.FormatNumber("N2"); // "1,234,567.89"
string formattedCurrency = 1234.56.FormatCurrency("USD"); // "$1,234.56"
string formattedDate = DateTime.Now.FormatDate("yyyy-MM-dd"); // "2024-03-20"
string formattedTime = DateTime.Now.FormatTime("HH:mm:ss"); // "14:30:45"

// String Manipulation
string truncated = "Hello World".Truncate(5); // "Hello..."
string reversed = "Hello".Reverse(); // "olleH"
string[] words = "Hello World".SplitWords(); // ["Hello", "World"]
string cleaned = "Hello   World".CleanWhitespace(); // "Hello World"

// Collection Operations
var distinct = list.DistinctBy(x => x.Id);
var grouped = list.GroupBy(x => x.Category);
var paginated = list.Paginate(page: 1, pageSize: 10);
var shuffled = list.Shuffle();
```

### Security

This module provides essential security features for your .NET applications, including password hashing, JWT token management, data encryption, and input sanitization.

## Features

- Password hashing and verification
- JWT token generation and validation
- Data encryption and decryption
- HTML and SQL input sanitization
- Secure random number generation
- Password strength validation
- XSS protection
- CSRF protection
- Secure file operations
- Cryptographic operations

## Installation

```bash
dotnet add package DeveloperHelper.Security
```

## Usage

### Basic Configuration

```csharp
using DeveloperHelper.Security;

// Configure security service
services.AddSecurityService(options =>
{
    options.JwtSecret = "your-secret-key";
    options.JwtExpiration = TimeSpan.FromHours(1);
    options.EncryptionKey = "your-encryption-key";
    options.EnableXssProtection = true;
    options.EnableCsrfProtection = true;
});
```

### Password Operations

```csharp
public class UserService
{
    private readonly ISecurityService _securityService;

    public UserService(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<User> CreateUserAsync(UserRegistration registration)
    {
        // Hash password
        var hashedPassword = await _securityService.HashPasswordAsync(registration.Password);

        // Create user with hashed password
        var user = new User
        {
            Username = registration.Username,
            PasswordHash = hashedPassword,
            Email = registration.Email
        };

        return user;
    }

    public async Task<bool> ValidatePasswordAsync(string password, string passwordHash)
    {
        return await _securityService.VerifyPasswordAsync(password, passwordHash);
    }
}
```

### JWT Token Operations

```csharp
public class AuthService
{
    private readonly ISecurityService _securityService;

    public AuthService(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var claims = new Dictionary<string, string>
        {
            { "userId", user.Id.ToString() },
            { "username", user.Username },
            { "role", user.Role }
        };

        return await _securityService.GenerateJwtTokenAsync(claims);
    }

    public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
    {
        return await _securityService.ValidateJwtTokenAsync(token);
    }
}
```

### Data Encryption

```csharp
public class SensitiveDataService
{
    private readonly ISecurityService _securityService;

    public SensitiveDataService(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<string> EncryptSensitiveDataAsync(string data)
    {
        return await _securityService.EncryptAsync(data);
    }

    public async Task<string> DecryptSensitiveDataAsync(string encryptedData)
    {
        return await _securityService.DecryptAsync(encryptedData);
    }
}
```

## Features and Methods

### ISecurityService

- `HashPasswordAsync(string password)`: Hash a password
- `VerifyPasswordAsync(string password, string passwordHash)`: Verify a password
- `GenerateJwtTokenAsync(Dictionary<string, string> claims)`: Generate JWT token
- `ValidateJwtTokenAsync(string token)`: Validate JWT token
- `EncryptAsync(string data)`: Encrypt data
- `DecryptAsync(string encryptedData)`: Decrypt data
- `SanitizeHtmlAsync(string html)`: Sanitize HTML input
- `SanitizeSqlAsync(string sql)`: Sanitize SQL input
- `GenerateSecureRandomStringAsync(int length)`: Generate secure random string
- `ValidatePasswordStrengthAsync(string password)`: Validate password strength

## Configuration

### Security Service Configuration

```csharp
services.AddSecurityService(options =>
{
    options.JwtSecret = "your-secret-key";
    options.JwtExpiration = TimeSpan.FromHours(1);
    options.EncryptionKey = "your-encryption-key";
    options.EnableXssProtection = true;
    options.EnableCsrfProtection = true;
    options.PasswordMinLength = 8;
    options.PasswordRequireUppercase = true;
    options.PasswordRequireLowercase = true;
    options.PasswordRequireDigit = true;
    options.PasswordRequireSpecialCharacter = true;
    options.EnablePasswordHistory = true;
    options.MaxPasswordHistoryCount = 5;
});
```

## Example Usage Scenarios

### 1. User Authentication

```csharp
public class AuthController
{
    private readonly ISecurityService _securityService;
    private readonly IUserService _userService;

    public AuthController(ISecurityService securityService, IUserService userService)
    {
        _securityService = securityService;
        _userService = userService;
    }

    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        var user = await _userService.GetUserByUsernameAsync(request.Username);
        if (user == null)
            return Unauthorized();

        var isValid = await _securityService.VerifyPasswordAsync(request.Password, user.PasswordHash);
        if (!isValid)
            return Unauthorized();

        var token = await _securityService.GenerateJwtTokenAsync(new Dictionary<string, string>
        {
            { "userId", user.Id.ToString() },
            { "role", user.Role }
        });

        return Ok(new { token });
    }
}
```

### 2. Secure Data Storage

```csharp
public class CreditCardService
{
    private readonly ISecurityService _securityService;
    private readonly ICreditCardRepository _repository;

    public CreditCardService(ISecurityService securityService, ICreditCardRepository repository)
    {
        _securityService = securityService;
        _repository = repository;
    }

    public async Task<CreditCard> SaveCreditCardAsync(CreditCard card)
    {
        // Encrypt sensitive data
        card.CardNumber = await _securityService.EncryptAsync(card.CardNumber);
        card.Cvv = await _securityService.EncryptAsync(card.Cvv);

        // Sanitize other data
        card.CardholderName = await _securityService.SanitizeHtmlAsync(card.CardholderName);

        return await _repository.SaveAsync(card);
    }

    public async Task<CreditCard> GetCreditCardAsync(int id)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null)
            return null;

        // Decrypt sensitive data
        card.CardNumber = await _securityService.DecryptAsync(card.CardNumber);
        card.Cvv = await _securityService.DecryptAsync(card.Cvv);

        return card;
    }
}
```

## Best Practices

1. **Password Security**
   - Use strong password policies
   - Implement password history
   - Use secure hashing algorithms
   - Implement rate limiting

2. **Token Management**
   - Use short-lived tokens
   - Implement refresh tokens
   - Secure token storage
   - Handle token revocation

3. **Data Protection**
   - Encrypt sensitive data
   - Use secure key management
   - Implement proper key rotation
   - Secure data transmission

4. **Input Validation**
   - Sanitize all user input
   - Validate data formats
   - Implement proper error handling
   - Use parameterized queries

5. **Security Headers**
   - Set appropriate security headers
   - Enable XSS protection
   - Enable CSRF protection
   - Implement CORS policies

## License

This project is licensed under the MIT License - see the LICENSE file for details. 