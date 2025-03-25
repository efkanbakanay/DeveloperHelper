using System;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace DeveloperHelper
{
    /// <summary>
    /// Dokümantasyon oluşturma ve güncelleme işlemleri için yardımcı sınıf.
    /// </summary>
    public static class DocumentationHelper
    {
        /// <summary>
        /// XML belgeleme yorumlarından otomatik olarak dokümantasyon oluşturur.
        /// </summary>
        /// <param name="assembly">Dokümantasyonu oluşturulacak assembly.</param>
        public static void GenerateDocumentation(Assembly assembly)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine("1. Assembly'den tipleri alınıyor...");
                var types = assembly.GetTypes()
                    .Where(t => t.IsPublic && !t.IsNested)
                    .OrderBy(t => t.Name)
                    .ToList();
                Console.WriteLine($"   ✓ {types.Count} tip bulundu ({sw.ElapsedMilliseconds}ms)");

                var doc = new StringBuilder(10000);
                doc.AppendLine("# DeveloperHelper Library Documentation\n");

                int methodCount = 0;
                foreach (var type in types)
                {
                    doc.AppendLine($"## {type.Name}\n");

                    var typeDoc = type.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                    if (typeDoc != null)
                    {
                        doc.AppendLine($"{typeDoc.Description}\n");
                    }

                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .Where(m => !m.IsSpecialName)
                        .OrderBy(m => m.Name)
                        .ToList();

                    methodCount += methods.Count;
                    foreach (var method in methods)
                    {
                        doc.AppendLine($"### {method.Name}\n");

                        var methodDoc = method.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                        if (methodDoc != null)
                        {
                            doc.AppendLine($"{methodDoc.Description}\n");
                        }

                        doc.AppendLine("```csharp");
                        doc.AppendLine(GetMethodSignature(method));
                        doc.AppendLine("```\n");
                    }
                }
                Console.WriteLine($"2. Dokümantasyon içeriği oluşturuldu: {methodCount} metod ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("3. Dosyaya yazılıyor...");
                File.WriteAllText("Documentation.md", doc.ToString());
                Console.WriteLine($"   ✓ Dosya yazıldı ({sw.ElapsedMilliseconds}ms)");

                sw.Stop();
                Console.WriteLine($"✅ Dokümantasyon başarıyla oluşturuldu! Toplam süre: {sw.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                sw.Stop();
                Console.WriteLine($"❌ Hata: {ex.Message}");
                Console.WriteLine($"Hata oluştuğu noktadaki süre: {sw.ElapsedMilliseconds}ms");
                throw;
            }
        }

        private static string GetMethodSignature(MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 0)
                return $"public {(method.IsStatic ? "static " : "")}{method.ReturnType.Name} {method.Name}()";

            var paramStrings = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                paramStrings[i] = $"{p.ParameterType.Name} {p.Name}";
            }

            return $"public {(method.IsStatic ? "static " : "")}{method.ReturnType.Name} {method.Name}({string.Join(", ", paramStrings)})";
        }
    }
} 