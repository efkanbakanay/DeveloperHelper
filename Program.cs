using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DeveloperHelper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                Console.WriteLine("Program başlatılıyor...");
                Console.WriteLine($"Çalışma dizini: {Directory.GetCurrentDirectory()}");
                Console.WriteLine($"Argümanlar: {string.Join(", ", args)}");

                if (args.Length > 0 && args[0] == "--generate-docs")
                {
                    Console.WriteLine("Assembly yükleniyor...");
                    var assembly = typeof(Program).Assembly;
                    Console.WriteLine($"Assembly yüklendi: {assembly.FullName}");
                    Console.WriteLine($"Assembly konumu: {assembly.Location}");

                    var types = assembly.GetTypes();
                    Console.WriteLine($"Toplam tip sayısı: {types.Length}");
                    Console.WriteLine($"Public tip sayısı: {types.Count(t => t.IsPublic)}");

                    Console.WriteLine("Dokümantasyon oluşturuluyor...");
                    DocumentationHelper.GenerateDocumentation(assembly);
                }
                else
                {
                    Console.WriteLine("Dokümantasyon oluşturmak için --generate-docs parametresini kullanın.");
                }

                sw.Stop();
                Console.WriteLine($"Program tamamlandı. Toplam süre: {sw.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"Inner Exception: {ex.InnerException}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"Target Site: {ex.TargetSite}");
                Environment.Exit(1);
            }
        }
    }
} 