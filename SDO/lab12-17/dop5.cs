using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

public class DuplicateFileFinder
{
    public void FindAndManageDuplicates(string targetDirectory, bool includeSubdirectories = true)
    {
        if (!Directory.Exists(targetDirectory))
        {
            Console.WriteLine("Указанная папка не существует.");
            return;
        }

        SearchOption searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var allFiles = Directory.GetFiles(targetDirectory, "*.*", searchOption)
                                .Select(path => new FileInfo(path))
                                .ToList();

        // Шаг 1: Группируем по размеру (файлы разного размера не могут быть дубликатами)
        var sizeGroups = allFiles.GroupBy(f => f.Length)
                                 .Where(g => g.Count() > 1);

        var duplicateGroups = new List<List<FileInfo>>();

        // Шаг 2: Для файлов одинакового размера вычисляем хеш содержимого
        foreach (var group in sizeGroups)
        {
            var hashGroups = group.GroupBy(f => ComputeHash(f.FullName))
                                  .Where(g => g.Count() > 1);

            foreach (var hashGroup in hashGroups)
            {
                duplicateGroups.Add(hashGroup.ToList());
            }
        }

        if (!duplicateGroups.Any())
        {
            Console.WriteLine("Дубликаты не найдены.");
            return;
        }

        // Шаг 3: Вывод результатов и интерактивное удаление
        DisplayAndDeleteDuplicates(duplicateGroups);
    }

    private void DisplayAndDeleteDuplicates(List<List<FileInfo>> duplicateGroups)
    {
        for (int i = 0; i < duplicateGroups.Count; i++)
        {
            var group = duplicateGroups[i];
            Console.WriteLine($"\n--- Группа дубликатов #{i + 1} (Размер каждого: {group[0].Length:N0} байт) ---");

            for (int j = 0; j < group.Count; j++)
            {
                Console.WriteLine($"  [{j + 1}] {group[j].FullName}");
            }

            Console.Write("Введите номера файлов для удаления через запятую (или Enter, чтобы пропустить): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            var indicesToDelete = input.Split(',')
                                       .Select(s => int.TryParse(s.Trim(), out int idx) ? idx - 1 : -1)
                                       .Where(idx => idx >= 0 && idx < group.Count)
                                       .Distinct();

            foreach (int idx in indicesToDelete)
            {
                try
                {
                    group[idx].Delete();
                    Console.WriteLine($"  Удален: {group[idx].FullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Ошибка при удалении {group[idx].Name}: {ex.Message}");
                }
            }
        }
    }

    private string ComputeHash(string filePath)
    {
        using SHA256 sha256 = SHA256.Create();
        using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920);
        return Convert.ToHexString(sha256.ComputeHash(stream));
    }
}
