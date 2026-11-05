using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 10: обобщения ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: GenericComparator");
        Dop1.Run();

        Section("Дополнительное задание 2: ChainBuilder");
        Dop2.Run();

        Section("Дополнительное задание 3: MatrixTransformer");
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
    public interface IMatrix<T>
    {
        void Set(int row, int col, T value);
        T Get(int row, int col);
        int GetRows();
        int GetCols();
    }

    public class Matrix<T> : IMatrix<T> where T : struct, IComparable<T>
    {
        private readonly T[,] _data;

        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными.");
            _data = new T[rows, cols];
        }

        public int GetRows() => _data.GetLength(0);
        public int GetCols() => _data.GetLength(1);

        public void Set(int row, int col, T value)
        {
            ValidateIndexes(row, col);
            _data[row, col] = value;
        }

        public T Get(int row, int col)
        {
            ValidateIndexes(row, col);
            return _data[row, col];
        }

        public T FindMax()
        {
            if (_data.Length == 0) throw new InvalidOperationException("Матрица пуста.");
            T max = _data[0, 0];
            foreach (T value in _data)
            {
                if (value.CompareTo(max) > 0) max = value;
            }
            return max;
        }

        private void ValidateIndexes(int row, int col)
        {
            if (row < 0 || row >= GetRows() || col < 0 || col >= GetCols())
                throw new ArgumentOutOfRangeException($"Индекс ({row}, {col}) вне границ матрицы.");
        }
    }

    private static void TestMatrix<T>(string title) where T : struct, IComparable<T>
    {
        Console.WriteLine($"--- {title} ---");
        Matrix<T> matrix = new Matrix<T>(2, 3);
        try
        {
            matrix.Set(0, 0, (T)Convert.ChangeType(1, typeof(T)));
            matrix.Set(0, 1, (T)Convert.ChangeType(5, typeof(T)));
            matrix.Set(0, 2, (T)Convert.ChangeType(3, typeof(T)));
            matrix.Set(1, 0, (T)Convert.ChangeType(7, typeof(T)));
            matrix.Set(1, 1, (T)Convert.ChangeType(2, typeof(T)));
            matrix.Set(1, 2, (T)Convert.ChangeType(4, typeof(T)));

            Console.WriteLine($"Размер: {matrix.GetRows()}x{matrix.GetCols()}");
            Console.WriteLine($"Элемент (1, 0): {matrix.Get(1, 0)}");
            Console.WriteLine($"Максимум: {matrix.FindMax()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Работа с матрицей завершена.");
        }
    }

    public static void Run()
    {
        TestMatrix<int>("int");
        TestMatrix<float>("float");
        TestMatrix<double>("double");
    }
}
