# DeveloperHelper

DeveloperHelper is a comprehensive helper library designed to meet developers' daily needs.

## Features

- String Operations
  - Null/Empty checks
  - Title Case conversions
  - String formatting
  - Email, URL, GUID, Date validations

- File Operations
  - Directory creation
  - File reading/writing
  - File deletion

- Parse Operations
  - Generic type conversions
  - Safe parsing operations
  - Default value support

- Format Operations
  - Currency formatting
  - Date formatting
  - Phone number formatting

- Validation Operations
  - Entity validation
  - Property validation
  - Collection validation
  - Null checks
  - Numeric value checks
  - Custom validation rules

- Error Management
  - IHasError interface
  - ErrorTrackingEntity class
  - ValidationResult support

- Bulk Insert Operations
  - Generic bulk insert support

## Installation

```bash
dotnet add package DeveloperHelper
```

## Usage Examples

### String Operations
```csharp
string value = "hello world";
string titleCase = value.ToTitleCase(); // "Hello World"

// Validation examples
string email = "test@example.com";
bool isValidEmail = email.IsValidEmail();

string url = "https://example.com";
bool isValidUrl = url.IsValidUrl();
```

### Validation Operations
```csharp
// Entity validation
public class User
{
    [Required]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}

var user = new User { Name = "John", Email = "john@example.com" };
bool isValid = user.IsValid();
string errors = user.GetValidationErrors();

// Collection validation
var numbers = new List<int> { 1, 2, 3, 4, 5 };
bool hasItems = numbers.HasItems();
bool isValid = numbers.IsValid(x => x > 0);

// Numeric value checks
int number = 42;
bool isInRange = number.IsInRange(0, 100);
bool isPositive = number.IsPositive();
```

### Null Checks
```csharp
string value = null;
value.ThrowIfNull(nameof(value), "Value cannot be null");

// Collection null check
var list = new List<string>();
bool isEmpty = list.IsNullOrEmpty();
```

### File Operations
```csharp
await FileHelper.WriteAllTextAsync("test.txt", "Hello World");
string content = await FileHelper.ReadAllTextAsync("test.txt");
```

### Parse Operations
```csharp
int number = ParseHelper.ParseOrDefault("123", 0);
bool success = ParseHelper.TryParse("123", out int result);
```

### Format Operations
```csharp
string currency = FormatHelper.FormatCurrency(123.45m);
string date = FormatHelper.FormatDate(DateTime.Now);
string phone = FormatHelper.FormatPhoneNumber("5551234567");
```

### Error Management
```csharp
public class MyEntity : ErrorTrackingEntity
{
    [Required]
    public string Name { get; set; }
    
    public bool Validate()
    {
        return base.Validate();
    }
}
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details. 