using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DeveloperHelper
{
    /// <summary>
    /// DeveloperHelper kütüphanesi, C# geliştirme süreçlerinde sık kullanılan işlemleri kolaylaştıran,
    /// güvenli ve performanslı bir yardımcı kütüphanedir.
    /// </summary>
    /// <remarks>
    /// Bu kütüphane, günlük geliştirme süreçlerinde karşılaşılan yaygın problemleri çözmek için tasarlanmıştır.
    /// Tüm metodlar thread-safe olarak çalışır ve hata durumlarında güvenli bir şekilde işlem yapar.
    /// </remarks>
    public static class DeveloperHelper
    {
        /// <summary>
        /// Kütüphanenin versiyonunu döndürür.
        /// </summary>
        public const string Version = "1.0.0";

        /// <summary>
        /// Kütüphanenin minimum .NET versiyonunu döndürür.
        /// </summary>
        public const string MinimumDotNetVersion = "8.0";
    }

    /// <summary>
    /// Entity ve property validasyonları için yardımcı metodlar içeren sınıf.
    /// </summary>
    /// <remarks>
    /// Bu sınıf, Data Annotations kullanarak entity ve property validasyonlarını gerçekleştirir.
    /// Validasyon sonuçları detaylı hata mesajları içerir.
    /// </remarks>
    public static class ValidationHelper
    {
        /// <summary>
        /// Verilen entity'nin tüm validasyon kurallarına uygunluğunu kontrol eder.
        /// </summary>
        /// <typeparam name="T">Validasyonu yapılacak entity tipi.</typeparam>
        /// <param name="entity">Validasyonu yapılacak entity.</param>
        /// <returns>Validasyon sonucu. Başarılı ise ValidationResult.Success, değilse hata mesajı içeren ValidationResult.</returns>
        public static ValidationResult ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                return new ValidationResult(string.Join(", ", results.Select(r => r.ErrorMessage)));
            }
            
            return ValidationResult.Success;
        }

        /// <summary>
        /// Entity'nin validasyon kurallarına uygunluğunu kontrol eder.
        /// </summary>
        /// <typeparam name="T">Validasyonu yapılacak entity tipi.</typeparam>
        /// <param name="entity">Validasyonu yapılacak entity.</param>
        /// <returns>Entity validasyon kurallarına uygun ise true, değilse false.</returns>
        public static bool IsValid<T>(this T entity) where T : class
        {
            return ValidateEntity(entity) == ValidationResult.Success;
        }

        /// <summary>
        /// Entity'nin validasyon hatalarını string olarak döndürür.
        /// </summary>
        /// <typeparam name="T">Validasyonu yapılacak entity tipi.</typeparam>
        /// <param name="entity">Validasyonu yapılacak entity.</param>
        /// <returns>Validasyon hataları varsa hata mesajları, yoksa boş string.</returns>
        public static string GetValidationErrors<T>(this T entity) where T : class
        {
            var result = ValidateEntity(entity);
            return result?.ErrorMessage ?? string.Empty;
        }

        /// <summary>
        /// Belirli bir property'nin validasyon kurallarına uygunluğunu kontrol eder.
        /// </summary>
        /// <typeparam name="T">Property'nin tipi.</typeparam>
        /// <param name="value">Validasyonu yapılacak değer.</param>
        /// <param name="propertyName">Property'nin adı.</param>
        /// <returns>Validasyon sonuçları listesi.</returns>
        public static IEnumerable<ValidationResult> ValidateProperty<T>(T value, string propertyName)
        {
            var context = new ValidationContext(value) { MemberName = propertyName };
            var results = new List<ValidationResult>();
            
            Validator.TryValidateProperty(value, context, results);
            return results;
        }
    }

    /// <summary>
    /// String işlemleri için extension metodlar içeren sınıf.
    /// </summary>
    /// <remarks>
    /// Bu sınıf, string manipülasyonları için yaygın kullanılan metodları extension method olarak sunar.
    /// Tüm metodlar null-safe olarak çalışır.
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// String'in null veya boş olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek string.</param>
        /// <returns>String null veya boş ise true, değilse false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// String'in null, boş veya sadece boşluk karakterlerinden oluşup oluşmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek string.</param>
        /// <returns>String null, boş veya sadece boşluk karakterlerinden oluşuyorsa true, değilse false.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// String'i Title Case formatına dönüştürür (Her kelimenin ilk harfi büyük).
        /// </summary>
        /// <param name="value">Dönüştürülecek string.</param>
        /// <returns>Title Case formatında string.</returns>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// String'in geçerli bir email adresi olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="email">Kontrol edilecek email adresi.</param>
        /// <returns>Geçerli email formatı ise true, değilse false.</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(email, 
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        /// <summary>
        /// String'in geçerli bir telefon numarası olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="phoneNumber">Kontrol edilecek telefon numarası.</param>
        /// <returns>Geçerli telefon numarası formatı ise true, değilse false.</returns>
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return digits.Length == 10;
        }

        /// <summary>
        /// String'in geçerli bir URL olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="url">Kontrol edilecek URL.</param>
        /// <returns>Geçerli URL formatı ise true, değilse false.</returns>
        public static bool IsValidUrl(this string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        /// <summary>
        /// String'in geçerli bir GUID olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="guid">Kontrol edilecek GUID.</param>
        /// <returns>Geçerli GUID formatı ise true, değilse false.</returns>
        public static bool IsValidGuid(this string guid)
        {
            return Guid.TryParse(guid, out _);
        }

        /// <summary>
        /// String'in geçerli bir tarih olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="date">Kontrol edilecek tarih string'i.</param>
        /// <returns>Geçerli tarih formatı ise true, değilse false.</returns>
        public static bool IsValidDate(this string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }

    /// <summary>
    /// Koleksiyon işlemleri için extension metodlar içeren sınıf.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Koleksiyonun null veya boş olup olmadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Koleksiyon elemanlarının tipi.</typeparam>
        /// <param name="collection">Kontrol edilecek koleksiyon.</param>
        /// <returns>Koleksiyon null veya boş ise true, değilse false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Koleksiyonun eleman içerip içermediğini kontrol eder.
        /// </summary>
        /// <typeparam name="T">Koleksiyon elemanlarının tipi.</typeparam>
        /// <param name="collection">Kontrol edilecek koleksiyon.</param>
        /// <returns>Koleksiyon eleman içeriyorsa true, değilse false.</returns>
        public static bool HasItems<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        /// <summary>
        /// Koleksiyonun belirtilen koşulu sağlayan eleman içerip içermediğini kontrol eder.
        /// </summary>
        /// <typeparam name="T">Koleksiyon elemanlarının tipi.</typeparam>
        /// <param name="collection">Kontrol edilecek koleksiyon.</param>
        /// <param name="predicate">Kontrol edilecek koşul.</param>
        /// <returns>Koleksiyon koşulu sağlayan eleman içeriyorsa true, değilse false.</returns>
        public static bool HasItems<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection != null && collection.Any(predicate);
        }

        /// <summary>
        /// Koleksiyonun tüm elemanlarının belirtilen validasyon koşulunu sağlayıp sağlamadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Koleksiyon elemanlarının tipi.</typeparam>
        /// <param name="collection">Kontrol edilecek koleksiyon.</param>
        /// <param name="validator">Validasyon koşulu.</param>
        /// <returns>Tüm elemanlar validasyon koşulunu sağlıyorsa true, değilse false.</returns>
        public static bool IsValid<T>(this IEnumerable<T> collection, Func<T, bool> validator)
        {
            return collection != null && collection.All(validator);
        }
    }

    /// <summary>
    /// Nesne işlemleri için extension metodlar içeren sınıf.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Nesnenin null olup olmadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Nesnenin tipi.</typeparam>
        /// <param name="obj">Kontrol edilecek nesne.</param>
        /// <returns>Nesne null ise true, değilse false.</returns>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// Nesnenin null olmadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Nesnenin tipi.</typeparam>
        /// <param name="obj">Kontrol edilecek nesne.</param>
        /// <returns>Nesne null değilse true, null ise false.</returns>
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        /// <summary>
        /// Nesne null ise ArgumentNullException fırlatır.
        /// </summary>
        /// <typeparam name="T">Nesnenin tipi.</typeparam>
        /// <param name="obj">Kontrol edilecek nesne.</param>
        /// <param name="paramName">Parametre adı.</param>
        /// <returns>Nesne null değilse nesnenin kendisi.</returns>
        /// <exception cref="ArgumentNullException">Nesne null ise fırlatılır.</exception>
        public static T ThrowIfNull<T>(this T obj, string paramName) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return obj;
        }

        /// <summary>
        /// Nesne null ise özel mesaj ile ArgumentNullException fırlatır.
        /// </summary>
        /// <typeparam name="T">Nesnenin tipi.</typeparam>
        /// <param name="obj">Kontrol edilecek nesne.</param>
        /// <param name="paramName">Parametre adı.</param>
        /// <param name="message">Hata mesajı.</param>
        /// <returns>Nesne null değilse nesnenin kendisi.</returns>
        /// <exception cref="ArgumentNullException">Nesne null ise fırlatılır.</exception>
        public static T ThrowIfNull<T>(this T obj, string paramName, string message) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, message);
            return obj;
        }

        /// <summary>
        /// Nesnenin belirtilen validasyon koşulunu sağlayıp sağlamadığını kontrol eder.
        /// </summary>
        /// <typeparam name="T">Nesnenin tipi.</typeparam>
        /// <param name="obj">Kontrol edilecek nesne.</param>
        /// <param name="validator">Validasyon koşulu.</param>
        /// <returns>Nesne validasyon koşulunu sağlıyorsa true, değilse false.</returns>
        public static bool IsValid<T>(this T obj, Func<T, bool> validator)
        {
            return obj != null && validator(obj);
        }
    }

    /// <summary>
    /// Sayısal işlemler için extension metodlar içeren sınıf.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Sayının belirtilen aralıkta olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek sayı.</param>
        /// <param name="min">Minimum değer.</param>
        /// <param name="max">Maksimum değer.</param>
        /// <returns>Sayı belirtilen aralıkta ise true, değilse false.</returns>
        public static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Decimal sayının belirtilen aralıkta olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek decimal sayı.</param>
        /// <param name="min">Minimum değer.</param>
        /// <param name="max">Maksimum değer.</param>
        /// <returns>Sayı belirtilen aralıkta ise true, değilse false.</returns>
        public static bool IsInRange(this decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Sayının pozitif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek sayı.</param>
        /// <returns>Sayı pozitif ise true, değilse false.</returns>
        public static bool IsPositive(this int value)
        {
            return value > 0;
        }

        /// <summary>
        /// Decimal sayının pozitif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek decimal sayı.</param>
        /// <returns>Sayı pozitif ise true, değilse false.</returns>
        public static bool IsPositive(this decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// Sayının negatif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek sayı.</param>
        /// <returns>Sayı negatif ise true, değilse false.</returns>
        public static bool IsNegative(this int value)
        {
            return value < 0;
        }

        /// <summary>
        /// Decimal sayının negatif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek decimal sayı.</param>
        /// <returns>Sayı negatif ise true, değilse false.</returns>
        public static bool IsNegative(this decimal value)
        {
            return value < 0;
        }
    }

    /// <summary>
    /// Dosya işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Belirtilen dizinin var olup olmadığını kontrol eder, yoksa oluşturur.
        /// </summary>
        /// <param name="path">Oluşturulacak dizin yolu.</param>
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Belirtilen dosyaya asenkron olarak içerik yazar.
        /// </summary>
        /// <param name="path">Dosya yolu.</param>
        /// <param name="content">Yazılacak içerik.</param>
        /// <returns>İşlem tamamlandığında tamamlanan Task.</returns>
        public static async Task WriteAllTextAsync(string path, string content)
        {
            await File.WriteAllTextAsync(path, content);
        }

        /// <summary>
        /// Belirtilen dosyayı asenkron olarak okur.
        /// </summary>
        /// <param name="path">Dosya yolu.</param>
        /// <returns>Dosya içeriği.</returns>
        public static async Task<string> ReadAllTextAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        /// <summary>
        /// Belirtilen dosyanın var olup olmadığını kontrol eder, varsa siler.
        /// </summary>
        /// <param name="path">Silinecek dosya yolu.</param>
        public static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    /// <summary>
    /// Parse işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class ParseHelper
    {
        /// <summary>
        /// String'i belirtilen tipe dönüştürür, başarısız olursa varsayılan değeri döndürür.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek tip.</typeparam>
        /// <param name="value">Dönüştürülecek string.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa döndürülecek varsayılan değer.</param>
        /// <returns>Dönüştürülmüş değer veya varsayılan değer.</returns>
        public static T ParseOrDefault<T>(string? value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// String'i belirtilen tipe dönüştürmeyi dener.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek tip.</typeparam>
        /// <param name="value">Dönüştürülecek string.</param>
        /// <param name="result">Dönüştürülen değer.</param>
        /// <returns>Dönüşüm başarılı ise true, değilse false.</returns>
        public static bool TryParse<T>(string value, out T result)
        {
            result = default;
            if (string.IsNullOrEmpty(value)) return false;

            try
            {
                result = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Format işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// Para birimini formatlar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="currencySymbol">Para birimi sembolü.</param>
        /// <returns>Formatlanmış para birimi string'i.</returns>
        public static string FormatCurrency(decimal value, string currencySymbol = "₺")
        {
            return $"{currencySymbol}{value:N2}";
        }

        /// <summary>
        /// Tarihi belirtilen formatta formatlar.
        /// </summary>
        /// <param name="date">Formatlanacak tarih.</param>
        /// <param name="format">Tarih formatı.</param>
        /// <returns>Formatlanmış tarih string'i.</returns>
        public static string FormatDate(DateTime date, string format = "dd.MM.yyyy")
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Telefon numarasını formatlar.
        /// </summary>
        /// <param name="phoneNumber">Formatlanacak telefon numarası.</param>
        /// <returns>Formatlanmış telefon numarası.</returns>
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return phoneNumber;
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return phoneNumber.Length == 10 ? $"{phoneNumber.Substring(0, 3)} {phoneNumber.Substring(3, 3)} {phoneNumber.Substring(6)}" : phoneNumber;
        }
    }

    /// <summary>
    /// Hata durumlarını takip etmek için kullanılan interface.
    /// </summary>
    public interface IHasError
    {
        /// <summary>
        /// Nesnenin hata durumunu belirtir.
        /// </summary>
        bool HasError { get; set; }

        /// <summary>
        /// Hata mesajını belirtir.
        /// </summary>
        string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Hata durumlarını ve validasyon sonuçlarını takip eden temel entity sınıfı.
    /// </summary>
    public class ErrorTrackingEntity : IHasError
    {
        /// <summary>
        /// Nesnenin hata durumunu belirtir.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Hata mesajını belirtir.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Validasyon hatalarını içeren liste.
        /// </summary>
        public List<ValidationResult> ValidationErrors { get; set; } = new List<ValidationResult>();

        /// <summary>
        /// Entity'nin validasyonunu gerçekleştirir.
        /// </summary>
        /// <returns>Validasyon başarılı ise true, değilse false.</returns>
        public bool Validate()
        {
            var result = ValidationHelper.ValidateEntity(this);
            HasError = result != ValidationResult.Success;
            if (HasError)
            {
                ErrorMessage = result.ErrorMessage;
                ValidationErrors.Add(result);
            }
            return !HasError;
        }
    }

    /// <summary>
    /// Toplu veri ekleme işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class BulkInsertHelper
    {
        /// <summary>
        /// Toplu veri ekleme sonucunu temsil eden sınıf.
        /// </summary>
        public class BulkInsertResult<T> where T : class
        {
            /// <summary>
            /// Başarıyla eklenen/güncellenen kayıtlar.
            /// </summary>
            public List<T> SuccessItems { get; set; } = new List<T>();

            /// <summary>
            /// Hata alan kayıtlar ve hata bilgileri.
            /// </summary>
            public List<BulkInsertError<T>> FailedItems { get; set; } = new List<BulkInsertError<T>>();

            /// <summary>
            /// Toplam işlem sayısı.
            /// </summary>
            public int TotalCount { get; set; }

            /// <summary>
            /// Başarılı işlem sayısı.
            /// </summary>
            public int SuccessCount => SuccessItems.Count;

            /// <summary>
            /// Başarısız işlem sayısı.
            /// </summary>
            public int FailedCount => FailedItems.Count;

            /// <summary>
            /// İşlemin başarılı olup olmadığı.
            /// </summary>
            public bool IsSuccess => FailedCount == 0;
        }

        /// <summary>
        /// Hata alan kayıt bilgilerini temsil eden sınıf.
        /// </summary>
        public class BulkInsertError<T> where T : class
        {
            /// <summary>
            /// Hata alan kayıt.
            /// </summary>
            public T? Item { get; set; }

            /// <summary>
            /// Hata mesajı.
            /// </summary>
            public string? ErrorMessage { get; set; }

            /// <summary>
            /// Hata kodu.
            /// </summary>
            public string? ErrorCode { get; set; }

            /// <summary>
            /// Hata detayları.
            /// </summary>
            public Dictionary<string, object> ErrorDetails { get; set; } = new Dictionary<string, object>();
        }

        /// <summary>
        /// Verilen entity listesini veritabanına toplu olarak ekler.
        /// </summary>
        /// <typeparam name="T">Eklenecek entity tipi.</typeparam>
        /// <param name="entities">Eklenecek entity'ler.</param>
        /// <param name="connectionString">Veritabanı bağlantı string'i.</param>
        /// <param name="tableName">Hedef tablo adı.</param>
        /// <returns>İşlem sonucu detayları.</returns>
        /// <exception cref="NotImplementedException">Bu metod implementasyonu için Dapper veya başka bir ORM kullanılmalıdır.</exception>
        public static async Task<BulkInsertResult<T>> BulkInsertAsync<T>(IEnumerable<T> entities, string connectionString, string tableName) where T : class
        {
            throw new NotImplementedException("Bu metod implementasyonu için Dapper veya başka bir ORM kullanılmalıdır.");
        }

        /// <summary>
        /// Verilen entity listesini veritabanına toplu olarak ekler veya günceller.
        /// </summary>
        /// <typeparam name="T">Eklenecek/güncellenecek entity tipi.</typeparam>
        /// <param name="entities">Eklenecek/güncellenecek entity'ler.</param>
        /// <param name="connectionString">Veritabanı bağlantı string'i.</param>
        /// <param name="tableName">Hedef tablo adı.</param>
        /// <param name="updateExisting">Var olan kayıtların güncellenip güncellenmeyeceği.</param>
        /// <returns>İşlem sonucu detayları.</returns>
        /// <exception cref="NotImplementedException">Bu metod implementasyonu için Dapper veya başka bir ORM kullanılmalıdır.</exception>
        public static async Task<BulkInsertResult<T>> BulkUpsertAsync<T>(IEnumerable<T> entities, string connectionString, string tableName, bool updateExisting = true) where T : class
        {
            throw new NotImplementedException("Bu metod implementasyonu için Dapper veya başka bir ORM kullanılmalıdır.");
        }

        /// <summary>
        /// Hata alan kayıtları tekrar işlemeye çalışır.
        /// </summary>
        /// <typeparam name="T">Entity tipi.</typeparam>
        /// <param name="failedItems">Hata alan kayıtlar.</param>
        /// <param name="connectionString">Veritabanı bağlantı string'i.</param>
        /// <param name="tableName">Hedef tablo adı.</param>
        /// <returns>Yeni işlem sonucu detayları.</returns>
        public static async Task<BulkInsertResult<T>> RetryFailedItemsAsync<T>(IEnumerable<BulkInsertError<T>> failedItems, string connectionString, string tableName) where T : class
        {
            var items = failedItems.Where(x => x.Item != null).Select(x => x.Item!);
            return await BulkInsertAsync(items, connectionString, tableName);
        }
    }

    /// <summary>
    /// Serialization işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Nesneyi XML formatına dönüştürür.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi.</typeparam>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <param name="includeNamespaces">XML namespace'lerinin dahil edilip edilmeyeceği.</param>
        /// <returns>XML formatında string.</returns>
        public static string ToXml<T>(T obj, bool includeNamespaces = false) where T : class
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                var settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = !includeNamespaces
                };

                using (var xmlWriter = System.Xml.XmlWriter.Create(writer, settings))
                {
                    if (includeNamespaces)
                    {
                        var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                        ns.Add("", "");
                        serializer.Serialize(xmlWriter, obj, ns);
                    }
                    else
                    {
                        serializer.Serialize(xmlWriter, obj);
                    }
                }
                return writer.ToString();
            }
        }

        /// <summary>
        /// XML string'ini nesneye dönüştürür.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi.</typeparam>
        /// <param name="xml">XML formatında string.</param>
        /// <returns>Dönüştürülmüş nesne.</returns>
        public static T FromXml<T>(string xml) where T : class
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Nesneyi JSON formatına dönüştürür.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi.</typeparam>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <param name="indent">JSON'ın formatlanıp formatlanmayacağı.</param>
        /// <returns>JSON formatında string.</returns>
        public static string ToJson<T>(T obj, bool indent = false) where T : class
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = indent
            };
            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// JSON string'ini nesneye dönüştürür.
        /// </summary>
        /// <typeparam name="T">Dönüştürülecek nesne tipi.</typeparam>
        /// <param name="json">JSON formatında string.</param>
        /// <returns>Dönüştürülmüş nesne.</returns>
        public static T FromJson<T>(string json) where T : class
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }

    /// <summary>
    /// Sayısal işlemler için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class NumericHelper
    {
        /// <summary>
        /// Sayının null veya 0 olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek sayı.</param>
        /// <returns>Sayı null veya 0 ise true, değilse false.</returns>
        public static bool IsNullOrZero(this int? value)
        {
            return !value.HasValue || value.Value == 0;
        }

        /// <summary>
        /// Decimal sayının null veya 0 olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek decimal sayı.</param>
        /// <returns>Sayı null veya 0 ise true, değilse false.</returns>
        public static bool IsNullOrZero(this decimal? value)
        {
            return !value.HasValue || value.Value == 0;
        }

        /// <summary>
        /// Sayının null, 0 veya negatif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek sayı.</param>
        /// <returns>Sayı null, 0 veya negatif ise true, değilse false.</returns>
        public static bool IsNullOrZeroOrNegative(this int? value)
        {
            return !value.HasValue || value.Value <= 0;
        }

        /// <summary>
        /// Decimal sayının null, 0 veya negatif olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek decimal sayı.</param>
        /// <returns>Sayı null, 0 veya negatif ise true, değilse false.</returns>
        public static bool IsNullOrZeroOrNegative(this decimal? value)
        {
            return !value.HasValue || value.Value <= 0;
        }

        /// <summary>
        /// Sayıyı belirtilen ondalık basamağa yuvarlar.
        /// </summary>
        /// <param name="value">Yuvarlanacak sayı.</param>
        /// <param name="decimals">Ondalık basamak sayısı.</param>
        /// <returns>Yuvarlanmış sayı.</returns>
        public static decimal RoundTo(this decimal value, int decimals)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Sayıyı yukarı yuvarlar.
        /// </summary>
        /// <param name="value">Yuvarlanacak sayı.</param>
        /// <param name="decimals">Ondalık basamak sayısı.</param>
        /// <returns>Yukarı yuvarlanmış sayı.</returns>
        public static decimal RoundUp(this decimal value, int decimals)
        {
            var multiplier = (decimal)Math.Pow(10, decimals);
            return Math.Ceiling(value * multiplier) / multiplier;
        }

        /// <summary>
        /// Sayıyı aşağı yuvarlar.
        /// </summary>
        /// <param name="value">Yuvarlanacak sayı.</param>
        /// <param name="decimals">Ondalık basamak sayısı.</param>
        /// <returns>Aşağı yuvarlanmış sayı.</returns>
        public static decimal RoundDown(this decimal value, int decimals)
        {
            var multiplier = (decimal)Math.Pow(10, decimals);
            return Math.Floor(value * multiplier) / multiplier;
        }
    }

    /// <summary>
    /// Para birimi işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class CurrencyHelper
    {
        /// <summary>
        /// Para birimi dönüşüm oranlarını içeren sınıf.
        /// </summary>
        public static class ExchangeRates
        {
            public const decimal USD_TO_EUR = 0.85m;
            public const decimal USD_TO_TRY = 31.50m;
            public const decimal EUR_TO_TRY = 37.00m;
            // Diğer dönüşüm oranları eklenebilir
        }

        /// <summary>
        /// Para birimini formatlar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="currencyCode">Para birimi kodu (USD, EUR, TRY vb.).</param>
        /// <param name="culture">Kültür bilgisi.</param>
        /// <returns>Formatlanmış para birimi string'i.</returns>
        public static string FormatCurrency(this decimal value, string currencyCode = "TRY", string culture = "tr-TR")
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            return value.ToString("C", cultureInfo);
        }

        /// <summary>
        /// Para birimini belirtilen formatta formatlar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="currencySymbol">Para birimi sembolü.</param>
        /// <param name="decimals">Ondalık basamak sayısı.</param>
        /// <returns>Formatlanmış para birimi string'i.</returns>
        public static string FormatCurrencyCustom(this decimal value, string currencySymbol = "₺", int decimals = 2)
        {
            return $"{currencySymbol}{value.RoundTo(decimals):N2}";
        }

        /// <summary>
        /// USD'den EUR'ya dönüştürür.
        /// </summary>
        /// <param name="usdAmount">USD miktarı.</param>
        /// <returns>EUR miktarı.</returns>
        public static decimal UsdToEur(this decimal usdAmount)
        {
            return usdAmount * ExchangeRates.USD_TO_EUR;
        }

        /// <summary>
        /// USD'den TRY'ye dönüştürür.
        /// </summary>
        /// <param name="usdAmount">USD miktarı.</param>
        /// <returns>TRY miktarı.</returns>
        public static decimal UsdToTry(this decimal usdAmount)
        {
            return usdAmount * ExchangeRates.USD_TO_TRY;
        }

        /// <summary>
        /// EUR'dan TRY'ye dönüştürür.
        /// </summary>
        /// <param name="eurAmount">EUR miktarı.</param>
        /// <returns>TRY miktarı.</returns>
        public static decimal EurToTry(this decimal eurAmount)
        {
            return eurAmount * ExchangeRates.EUR_TO_TRY;
        }

        /// <summary>
        /// Para birimini belirtilen para birimine dönüştürür.
        /// </summary>
        /// <param name="amount">Dönüştürülecek miktar.</param>
        /// <param name="fromCurrency">Kaynak para birimi.</param>
        /// <param name="toCurrency">Hedef para birimi.</param>
        /// <returns>Dönüştürülmüş miktar.</returns>
        public static decimal ConvertCurrency(this decimal amount, string fromCurrency, string toCurrency)
        {
            // Örnek dönüşüm mantığı
            if (fromCurrency == toCurrency) return amount;

            switch ($"{fromCurrency}-{toCurrency}")
            {
                case "USD-EUR":
                    return amount * ExchangeRates.USD_TO_EUR;
                case "USD-TRY":
                    return amount * ExchangeRates.USD_TO_TRY;
                case "EUR-TRY":
                    return amount * ExchangeRates.EUR_TO_TRY;
                // Diğer dönüşümler eklenebilir
                default:
                    throw new ArgumentException($"Desteklenmeyen para birimi dönüşümü: {fromCurrency} -> {toCurrency}");
            }
        }
    }

    /// <summary>
    /// Nesne ve liste işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class ObjectHelper
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Nesneyi null ise yeni bir nesne olarak oluşturur.
        /// </summary>
        public static T CreateIfNull<T>(this T obj) where T : class, new()
        {
            try
            {
                return obj ?? new T();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        /// <summary>
        /// Nesneyi null ise belirtilen factory metodunu kullanarak oluşturur.
        /// </summary>
        public static T CreateIfNull<T>(this T obj, Func<T> factory) where T : class
        {
            try
            {
                return obj ?? (factory?.Invoke() ?? throw new ArgumentNullException(nameof(factory)));
            }
            catch (Exception)
            {
                return factory?.Invoke() ?? throw new ArgumentNullException(nameof(factory));
            }
        }

        /// <summary>
        /// Nesneyi null ise belirtilen değer ile oluşturur.
        /// </summary>
        public static T CreateIfNull<T>(this T obj, T defaultValue) where T : class
        {
            try
            {
                return obj ?? (defaultValue ?? throw new ArgumentNullException(nameof(defaultValue)));
            }
            catch (Exception)
            {
                return defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
            }
        }

        /// <summary>
        /// Nesnenin belirtilen property'sinin değerini güvenli bir şekilde alır.
        /// </summary>
        public static TProperty GetPropertyValue<T, TProperty>(this T obj, Func<T, TProperty> propertySelector) where T : class
        {
            try
            {
                return obj != null && propertySelector != null ? propertySelector(obj) : default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Nesnenin belirtilen property'sinin değerini güvenli bir şekilde alır.
        /// </summary>
        public static TProperty GetPropertyValue<T, TProperty>(this T obj, Func<T, TProperty> propertySelector, TProperty defaultValue) where T : class
        {
            try
            {
                return obj != null && propertySelector != null ? propertySelector(obj) : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Nesnenin belirtilen property'sinin null olup olmadığını kontrol eder.
        /// </summary>
        public static bool IsPropertyNull<T, TProperty>(this T obj, Func<T, TProperty> propertySelector) where T : class
        {
            try
            {
                return obj == null || propertySelector == null || propertySelector(obj) == null;
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Nesnenin belirtilen property'sinin değerini kontrol eder.
        /// </summary>
        public static bool IsProperty<T, TProperty>(this T obj, Func<T, TProperty> propertySelector, Func<TProperty, bool> predicate) where T : class
        {
            try
            {
                return obj != null && propertySelector != null && predicate != null && predicate(propertySelector(obj));
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Nesnenin tüm property'lerini güvenli bir şekilde kopyalar.
        /// </summary>
        public static T Clone<T>(this T obj) where T : class, new()
        {
            try
            {
                if (obj == null) return null;

                var clone = new T();
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanWrite && p.CanRead);

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);
                    property.SetValue(clone, value);
                }

                return clone;
            }
            catch (Exception)
            {
                return new T();
            }
        }

        /// <summary>
        /// Nesnenin belirtilen property'lerini güvenli bir şekilde kopyalar.
        /// </summary>
        public static T Clone<T>(this T obj, params string[] propertyNames) where T : class, new()
        {
            try
            {
                if (obj == null || propertyNames == null || !propertyNames.Any()) return null;

                var clone = new T();
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanWrite && p.CanRead && propertyNames.Contains(p.Name));

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);
                    property.SetValue(clone, value);
                }

                return clone;
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }

    /// <summary>
    /// Liste işlemleri için yardımcı metodlar içeren sınıf.
    /// </summary>
    public static class ListHelper
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Listeyi null ise yeni bir liste olarak oluşturur.
        /// </summary>
        public static List<T> CreateIfNull<T>(this List<T> list)
        {
            try
            {
                return list ?? new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Listeyi null ise belirtilen elemanlarla oluşturur.
        /// </summary>
        public static List<T> CreateIfNull<T>(this List<T> list, params T[] items)
        {
            try
            {
                return list ?? (items != null ? new List<T>(items) : new List<T>());
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Listeyi null ise belirtilen koleksiyonla oluşturur.
        /// </summary>
        public static List<T> CreateIfNull<T>(this List<T> list, IEnumerable<T> collection)
        {
            try
            {
                return list ?? (collection != null ? new List<T>(collection) : new List<T>());
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Listeye eleman ekler ve null kontrolü yapar.
        /// </summary>
        public static List<T> AddIfNotNull<T>(this List<T> list, T item)
        {
            try
            {
                if (list == null)
                    list = new List<T>();
                
                if (item != null)
                {
                    lock (_lock)
                    {
                        list.Add(item);
                    }
                }
                
                return list;
            }
            catch (Exception)
            {
                return list ?? new List<T>();
            }
        }

        /// <summary>
        /// Listeye koleksiyon ekler ve null kontrolü yapar.
        /// </summary>
        public static List<T> AddRangeIfNotNull<T>(this List<T> list, IEnumerable<T> items)
        {
            try
            {
                if (list == null)
                    list = new List<T>();
                
                if (items != null)
                {
                    var validItems = items.Where(x => x != null).ToList();
                    if (validItems.Any())
                    {
                        lock (_lock)
                        {
                            list.AddRange(validItems);
                        }
                    }
                }
                
                return list;
            }
            catch (Exception)
            {
                return list ?? new List<T>();
            }
        }

        /// <summary>
        /// Listeden belirtilen koşulu sağlayan elemanları kaldırır.
        /// </summary>
        public static List<T> RemoveIf<T>(this List<T> list, Func<T, bool> predicate)
        {
            try
            {
                if (list != null && predicate != null)
                {
                    lock (_lock)
                    {
                        list.RemoveAll(x => predicate(x));
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// Listeden null elemanları kaldırır.
        /// </summary>
        public static List<T> RemoveNulls<T>(this List<T> list)
        {
            try
            {
                if (list != null)
                {
                    lock (_lock)
                    {
                        list.RemoveAll(x => x == null);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// Listeyi belirtilen sayıda elemana kısıtlar.
        /// </summary>
        public static List<T> LimitTo<T>(this List<T> list, int maxCount)
        {
            try
            {
                if (list != null && list.Count > maxCount)
                {
                    lock (_lock)
                    {
                        list.RemoveRange(maxCount, list.Count - maxCount);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// Listeyi belirtilen sayıda elemana kısıtlar ve fazla elemanları yeni bir listeye taşır.
        /// </summary>
        public static List<T> LimitTo<T>(this List<T> list, int maxCount, out List<T> overflowList)
        {
            overflowList = new List<T>();
            try
            {
                if (list != null && list.Count > maxCount)
                {
                    lock (_lock)
                    {
                        overflowList.AddRange(list.GetRange(maxCount, list.Count - maxCount));
                        list.RemoveRange(maxCount, list.Count - maxCount);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// Listeyi belirtilen sayıda parçaya böler.
        /// </summary>
        public static List<List<T>> Split<T>(this List<T> list, int parts)
        {
            try
            {
                if (list == null || !list.Any() || parts <= 0)
                    return new List<List<T>>();

                var result = new List<List<T>>();
                var itemsPerPart = (int)Math.Ceiling(list.Count / (double)parts);

                for (int i = 0; i < parts; i++)
                {
                    var startIndex = i * itemsPerPart;
                    if (startIndex >= list.Count)
                        break;

                    var part = list.Skip(startIndex).Take(itemsPerPart).ToList();
                    if (part.Any())
                        result.Add(part);
                }

                return result;
            }
            catch (Exception)
            {
                return new List<List<T>>();
            }
        }

        /// <summary>
        /// Listeyi belirtilen boyutta parçalara böler.
        /// </summary>
        public static List<List<T>> SplitBySize<T>(this List<T> list, int size)
        {
            try
            {
                if (list == null || !list.Any() || size <= 0)
                    return new List<List<T>>();

                var result = new List<List<T>>();
                for (int i = 0; i < list.Count; i += size)
                {
                    var part = list.Skip(i).Take(size).ToList();
                    if (part.Any())
                        result.Add(part);
                }

                return result;
            }
            catch (Exception)
            {
                return new List<List<T>>();
            }
        }
    }
} 