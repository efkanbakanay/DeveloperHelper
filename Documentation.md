# DeveloperHelper Library Documentation

## BulkInsertHelper

### BulkInsertAsync

```csharp
public static Task`1 BulkInsertAsync(IEnumerable`1 entities, String connectionString, String tableName)
```

### BulkUpsertAsync

```csharp
public static Task`1 BulkUpsertAsync(IEnumerable`1 entities, String connectionString, String tableName, Boolean updateExisting)
```

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### RetryFailedItemsAsync

```csharp
public static Task`1 RetryFailedItemsAsync(IEnumerable`1 failedItems, String connectionString, String tableName)
```

### ToString

```csharp
public String ToString()
```

## CollectionExtensions

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### HasItems

```csharp
public static Boolean HasItems(IEnumerable`1 collection)
```

### HasItems

```csharp
public static Boolean HasItems(IEnumerable`1 collection, Func`2 predicate)
```

### IsNullOrEmpty

```csharp
public static Boolean IsNullOrEmpty(IEnumerable`1 collection)
```

### IsValid

```csharp
public static Boolean IsValid(IEnumerable`1 collection, Func`2 validator)
```

### ToString

```csharp
public String ToString()
```

## CurrencyHelper

### ConvertCurrency

```csharp
public static Decimal ConvertCurrency(Decimal amount, String fromCurrency, String toCurrency)
```

### Equals

```csharp
public Boolean Equals(Object obj)
```

### EurToTry

```csharp
public static Decimal EurToTry(Decimal eurAmount)
```

### FormatCurrency

```csharp
public static String FormatCurrency(Decimal value, String currencyCode, String culture)
```

### FormatCurrencyCustom

```csharp
public static String FormatCurrencyCustom(Decimal value, String currencySymbol, Int32 decimals)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

### UsdToEur

```csharp
public static Decimal UsdToEur(Decimal usdAmount)
```

### UsdToTry

```csharp
public static Decimal UsdToTry(Decimal usdAmount)
```

## DeveloperHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

## DocumentationHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GenerateDocumentation

```csharp
public static Void GenerateDocumentation(Assembly assembly)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

## ErrorTrackingEntity

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

### Validate

```csharp
public Boolean Validate()
```

## FileHelper

### CreateDirectoryIfNotExists

```csharp
public static Void CreateDirectoryIfNotExists(String path)
```

### DeleteFileIfExists

```csharp
public static Void DeleteFileIfExists(String path)
```

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ReadAllTextAsync

```csharp
public static Task`1 ReadAllTextAsync(String path)
```

### ToString

```csharp
public String ToString()
```

### WriteAllTextAsync

```csharp
public static Task WriteAllTextAsync(String path, String content)
```

## FormatHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### FormatCurrency

```csharp
public static String FormatCurrency(Decimal value, String currencySymbol)
```

### FormatDate

```csharp
public static String FormatDate(DateTime date, String format)
```

### FormatPhoneNumber

```csharp
public static String FormatPhoneNumber(String phoneNumber)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

## IHasError

## ListHelper

### AddIfNotNull

```csharp
public static List`1 AddIfNotNull(List`1 list, T item)
```

### AddRangeIfNotNull

```csharp
public static List`1 AddRangeIfNotNull(List`1 list, IEnumerable`1 items)
```

### CreateIfNull

```csharp
public static List`1 CreateIfNull(List`1 list)
```

### CreateIfNull

```csharp
public static List`1 CreateIfNull(List`1 list, T[] items)
```

### CreateIfNull

```csharp
public static List`1 CreateIfNull(List`1 list, IEnumerable`1 collection)
```

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### LimitTo

```csharp
public static List`1 LimitTo(List`1 list, Int32 maxCount)
```

### LimitTo

```csharp
public static List`1 LimitTo(List`1 list, Int32 maxCount, List`1& overflowList)
```

### RemoveIf

```csharp
public static List`1 RemoveIf(List`1 list, Func`2 predicate)
```

### RemoveNulls

```csharp
public static List`1 RemoveNulls(List`1 list)
```

### Split

```csharp
public static List`1 Split(List`1 list, Int32 parts)
```

### SplitBySize

```csharp
public static List`1 SplitBySize(List`1 list, Int32 size)
```

### ToString

```csharp
public String ToString()
```

## NumberExtensions

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### IsInRange

```csharp
public static Boolean IsInRange(Int32 value, Int32 min, Int32 max)
```

### IsInRange

```csharp
public static Boolean IsInRange(Decimal value, Decimal min, Decimal max)
```

### IsNegative

```csharp
public static Boolean IsNegative(Int32 value)
```

### IsNegative

```csharp
public static Boolean IsNegative(Decimal value)
```

### IsPositive

```csharp
public static Boolean IsPositive(Int32 value)
```

### IsPositive

```csharp
public static Boolean IsPositive(Decimal value)
```

### ToString

```csharp
public String ToString()
```

## NumericHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### IsNullOrZero

```csharp
public static Boolean IsNullOrZero(Nullable`1 value)
```

### IsNullOrZero

```csharp
public static Boolean IsNullOrZero(Nullable`1 value)
```

### IsNullOrZeroOrNegative

```csharp
public static Boolean IsNullOrZeroOrNegative(Nullable`1 value)
```

### IsNullOrZeroOrNegative

```csharp
public static Boolean IsNullOrZeroOrNegative(Nullable`1 value)
```

### RoundDown

```csharp
public static Decimal RoundDown(Decimal value, Int32 decimals)
```

### RoundTo

```csharp
public static Decimal RoundTo(Decimal value, Int32 decimals)
```

### RoundUp

```csharp
public static Decimal RoundUp(Decimal value, Int32 decimals)
```

### ToString

```csharp
public String ToString()
```

## ObjectExtensions

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### IsNotNull

```csharp
public static Boolean IsNotNull(T obj)
```

### IsNull

```csharp
public static Boolean IsNull(T obj)
```

### IsValid

```csharp
public static Boolean IsValid(T obj, Func`2 validator)
```

### ThrowIfNull

```csharp
public static T ThrowIfNull(T obj, String paramName)
```

### ThrowIfNull

```csharp
public static T ThrowIfNull(T obj, String paramName, String message)
```

### ToString

```csharp
public String ToString()
```

## ObjectHelper

### Clone

```csharp
public static T Clone(T obj)
```

### Clone

```csharp
public static T Clone(T obj, String[] propertyNames)
```

### CreateIfNull

```csharp
public static T CreateIfNull(T obj)
```

### CreateIfNull

```csharp
public static T CreateIfNull(T obj, Func`1 factory)
```

### CreateIfNull

```csharp
public static T CreateIfNull(T obj, T defaultValue)
```

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetPropertyValue

```csharp
public static TProperty GetPropertyValue(T obj, Func`2 propertySelector)
```

### GetPropertyValue

```csharp
public static TProperty GetPropertyValue(T obj, Func`2 propertySelector, TProperty defaultValue)
```

### GetType

```csharp
public Type GetType()
```

### IsProperty

```csharp
public static Boolean IsProperty(T obj, Func`2 propertySelector, Func`2 predicate)
```

### IsPropertyNull

```csharp
public static Boolean IsPropertyNull(T obj, Func`2 propertySelector)
```

### ToString

```csharp
public String ToString()
```

## ParseHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ParseOrDefault

```csharp
public static T ParseOrDefault(String value, T defaultValue)
```

### ToString

```csharp
public String ToString()
```

### TryParse

```csharp
public static Boolean TryParse(String value, T& result)
```

## Program

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### Main

```csharp
public static Void Main(String[] args)
```

### ToString

```csharp
public String ToString()
```

## SecretsManager

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetNuGetApiKey

```csharp
public String GetNuGetApiKey()
```

### GetSecret

```csharp
public String GetSecret(String key)
```

### GetType

```csharp
public Type GetType()
```

### ToString

```csharp
public String ToString()
```

### TryGetSecret

```csharp
public Boolean TryGetSecret(String key, String& value)
```

## SerializationHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### FromJson

```csharp
public static T FromJson(String json)
```

### FromXml

```csharp
public static T FromXml(String xml)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### ToJson

```csharp
public static String ToJson(T obj, Boolean indent)
```

### ToString

```csharp
public String ToString()
```

### ToXml

```csharp
public static String ToXml(T obj, Boolean includeNamespaces)
```

## StringExtensions

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### IsNullOrEmpty

```csharp
public static Boolean IsNullOrEmpty(String value)
```

### IsNullOrWhiteSpace

```csharp
public static Boolean IsNullOrWhiteSpace(String value)
```

### IsValidDate

```csharp
public static Boolean IsValidDate(String date)
```

### IsValidEmail

```csharp
public static Boolean IsValidEmail(String email)
```

### IsValidGuid

```csharp
public static Boolean IsValidGuid(String guid)
```

### IsValidPhoneNumber

```csharp
public static Boolean IsValidPhoneNumber(String phoneNumber)
```

### IsValidUrl

```csharp
public static Boolean IsValidUrl(String url)
```

### ToString

```csharp
public String ToString()
```

### ToTitleCase

```csharp
public static String ToTitleCase(String value)
```

## ValidationHelper

### Equals

```csharp
public Boolean Equals(Object obj)
```

### GetHashCode

```csharp
public Int32 GetHashCode()
```

### GetType

```csharp
public Type GetType()
```

### GetValidationErrors

```csharp
public static String GetValidationErrors(T entity)
```

### IsValid

```csharp
public static Boolean IsValid(T entity)
```

### ToString

```csharp
public String ToString()
```

### ValidateEntity

```csharp
public static ValidationResult ValidateEntity(T entity)
```

### ValidateProperty

```csharp
public static IEnumerable`1 ValidateProperty(T value, String propertyName)
```

