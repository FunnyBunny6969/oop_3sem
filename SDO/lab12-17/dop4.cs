using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

public class FileSplitterMerger
{
    private const int BufferSize = 81920; // 80 КБ — оптимальный размер буфера

    // Разделение файла на части заданного размера (в байтах)
    public void SplitFile(string sourceFilePath, string outputDirectory, long partSizeInBytes)
    {
        if (!File.Exists(sourceFilePath))
            throw new FileNotFoundException("Исходный файл не найден.", sourceFilePath);

        Directory.CreateDirectory(outputDirectory);
        string baseFileName = Path.GetFileName(sourceFilePath);
        byte[] buffer = new byte[BufferSize];

        using FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
        int partNumber = 1;

        while (sourceStream.Position < sourceStream.Length)
        {
            string partFileName = Path.Combine(outputDirectory, $"{baseFileName}.part{partNumber:D4}");
            using FileStream partStream = new FileStream(partFileName, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize);

            long bytesRemainingInPart = partSizeInBytes;
            while (bytesRemainingInPart > 0 && sourceStream.Position < sourceStream.Length)
            {
                int bytesToRead = (int)Math.Min(buffer.Length, bytesRemainingInPart);
                int bytesRead = sourceStream.Read(buffer, 0, bytesToRead);

                if (bytesRead == 0) break;

                partStream.Write(buffer, 0, bytesRead);
                bytesRemainingInPart -= bytesRead;
            }

            Console.WriteLine($"Создана часть: {partFileName}");
            partNumber++;
        }
    }

    // Объединение частей обратно в один файл
    public void MergeFiles(string partsDirectory, string baseFileName, string destinationFilePath)
    {
        // Находим и сортируем части по порядку номеров
        var partFiles = Directory.GetFiles(partsDirectory, $"{baseFileName}.part*")
                                 .OrderBy(f => f)
                                 .ToList();

        if (!partFiles.Any())
            throw new FileNotFoundException("Файлы частей не найдены.");

        byte[] buffer = new byte[BufferSize];
        using FileStream destStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize);

        foreach (string partFile in partFiles)
        {
            using FileStream partStream = new FileStream(partFile, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
            int bytesRead;
            while ((bytesRead = partStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destStream.Write(buffer, 0, bytesRead);
            }
        }

        Console.WriteLine($"Файл успешно собран: {destinationFilePath}");
    }

    // Проверка идентичности двух файлов по SHA-256
    public bool AreFilesIdentical(string filePath1, string filePath2)
    {
        FileInfo info1 = new FileInfo(filePath1);
        FileInfo info2 = new FileInfo(filePath2);

        if (info1.Length != info2.Length)
            return false;

        string hash1 = ComputeFileHash(filePath1);
        string hash2 = ComputeFileHash(filePath2);

        return hash1 == hash2;
    }

    private string ComputeFileHash(string filePath)
    {
        using SHA256 sha256 = SHA256.Create();
        using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
        byte[] hashBytes = sha256.ComputeHash(stream);
        return Convert.ToHexString(hashBytes);
    }

    public void CreateDummyFile(string filePath, long sizeInBytes)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        byte[] buffer = new byte[81920]; // 80 КБ
        Random rnd = new Random();
        long bytesWritten = 0;

        while (bytesWritten < sizeInBytes)
        {
            rnd.NextBytes(buffer); // Заполняем буфер случайными данными
            int toWrite = (int)Math.Min(buffer.Length, sizeInBytes - bytesWritten);
            fs.Write(buffer, 0, toWrite);
            bytesWritten += toWrite;
        }
    }

}
