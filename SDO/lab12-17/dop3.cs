using System;
using System.Collections.Generic;
using System.IO;

public static class Dop3
{
    private static readonly Dictionary<string, string> FolderByExtension = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        [".txt"] = "Documents",
        [".pdf"] = "Documents",
        [".docx"] = "Documents",
        [".jpg"] = "Images",
        [".png"] = "Images",
        [".gif"] = "Images",
        [".mp3"] = "Audio",
        [".wav"] = "Audio",
        [".mp4"] = "Video",
        [".avi"] = "Video"
    };

    public static void Organize(string directory)
    {
        Directory.CreateDirectory(directory);

        foreach (string file in Directory.GetFiles(directory))
        {
            string extension = Path.GetExtension(file);
            string folderName = FolderByExtension.TryGetValue(extension, out string? folder) ? folder : "Other";
            string targetDirectory = Path.Combine(directory, folderName);
            Directory.CreateDirectory(targetDirectory);

            string fileName = Path.GetFileName(file);
            string targetPath = Path.Combine(targetDirectory, fileName);
            if (File.Exists(targetPath)) File.Delete(targetPath);
            File.Move(file, targetPath);
        }
    }

    public static void PrintTree(string directory)
    {
        foreach (string folder in Directory.GetDirectories(directory))
        {
            Console.WriteLine($"  {Path.GetFileName(folder)}/");
            foreach (string file in Directory.GetFiles(folder))
            {
                Console.WriteLine($"    {Path.GetFileName(file)}");
            }
        }

        foreach (string file in Directory.GetFiles(directory))
        {
            Console.WriteLine($"  {Path.GetFileName(file)}");
        }
    }

    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "lab12_17_organizer");
        if (Directory.Exists(directory)) Directory.Delete(directory, true);
        Directory.CreateDirectory(directory);

        string[] files = { "note.txt", "report.pdf", "photo.jpg", "music.mp3", "clip.mp4", "data.bin" };
        foreach (string file in files)
        {
            File.WriteAllText(Path.Combine(directory, file), "test");
        }

        Console.WriteLine("До организации:");
        PrintTree(directory);

        Organize(directory);
        Console.WriteLine("\nПосле организации:");
        PrintTree(directory);
    }
}
