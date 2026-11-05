using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 08: структуры и перечисления ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: космический корабль");
        Dop1.Run();

        Section("Дополнительное задание 2: умный дом");
        Dop2.Run();

        Section("Дополнительное задание 3: цифровая библиотека");
        Dop3.Run();
    }

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== " + title + " ===");
        Console.WriteLine(new string('=', 80));
    }
}

public static class MainTask
{
    public struct TimePeriod : IComparable<TimePeriod>
    {
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }

        public TimePeriod(int hours, int minutes, int seconds)
        {
            if (hours < 0 || hours > 23) throw new ArgumentOutOfRangeException(nameof(hours));
            if (minutes < 0 || minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes));
            if (seconds < 0 || seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds));
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public override string ToString() => $"{Hours:00}:{Minutes:00}:{Seconds:00}";

        public int CompareTo(TimePeriod other)
        {
            int result = Hours.CompareTo(other.Hours);
            if (result != 0) return result;
            result = Minutes.CompareTo(other.Minutes);
            if (result != 0) return result;
            return Seconds.CompareTo(other.Seconds);
        }

        public TimePeriod AddSeconds(int seconds)
        {
            int total = Hours * 3600 + Minutes * 60 + Seconds + seconds;
            total = ((total % 86400) + 86400) % 86400;
            return new TimePeriod(total / 3600, total % 3600 / 60, total % 60);
        }

        public static bool operator >(TimePeriod a, TimePeriod b) => a.CompareTo(b) > 0;
        public static bool operator <(TimePeriod a, TimePeriod b) => a.CompareTo(b) < 0;
    }

    public enum ChessPiece
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public static int GetPieceValue(ChessPiece piece) => piece switch
    {
        ChessPiece.Pawn => 1,
        ChessPiece.Knight => 3,
        ChessPiece.Bishop => 3,
        ChessPiece.Rook => 5,
        ChessPiece.Queen => 9,
        ChessPiece.King => 0,
        _ => 0
    };

    public static int GetTotalValue(params ChessPiece[] pieces)
    {
        int total = 0;
        foreach (ChessPiece piece in pieces) total += GetPieceValue(piece);
        return total;
    }

    public static string GetPieceName(ChessPiece piece) => piece switch
    {
        ChessPiece.Pawn => "Пешка",
        ChessPiece.Knight => "Конь",
        ChessPiece.Bishop => "Слон",
        ChessPiece.Rook => "Ладья",
        ChessPiece.Queen => "Ферзь",
        ChessPiece.King => "Король",
        _ => piece.ToString()
    };

    public static void Run()
    {
        TimePeriod first = new TimePeriod(14, 30, 15);
        TimePeriod second = new TimePeriod(9, 45, 50);

        Console.WriteLine($"Время 1: {first}");
        Console.WriteLine($"Время 2: {second}");
        Console.WriteLine($"Время 1 больше времени 2: {first > second}");
        Console.WriteLine($"Время 2 больше времени 1: {second > first}");
        Console.WriteLine($"К времени 1 добавлено 3700 секунд: {first.AddSeconds(3700)}");
        Console.WriteLine($"К времени 2 добавлено 30 секунд: {second.AddSeconds(30)}");

        Console.WriteLine();

        ChessPiece[] pieces =
        {
            ChessPiece.Pawn, ChessPiece.Knight, ChessPiece.Bishop,
            ChessPiece.Rook, ChessPiece.Queen, ChessPiece.King
        };

        foreach (ChessPiece piece in pieces)
        {
            Console.WriteLine($"{GetPieceName(piece)} — {GetPieceValue(piece)} очк.");
        }

        Console.WriteLine($"Общая стоимость набора: {GetTotalValue(pieces)} очк.");
    }
}
