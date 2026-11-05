using System;

public static class Dop3
{
    public enum FlipDirection { Horizontal, Vertical }

    public class MatrixTransformer<T> where T : struct
    {
        public T[,] Transpose(T[,] matrix)
        {
            Validate(matrix);
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            T[,] result = new T[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            return result;
        }

        public T[,] Rotate90(T[,] matrix)
        {
            Validate(matrix);
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            T[,] result = new T[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[j, rows - 1 - i] = matrix[i, j];
                }
            }
            return result;
        }

        public T[,] Flip(T[,] matrix, FlipDirection direction)
        {
            Validate(matrix);
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            T[,] result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (direction == FlipDirection.Horizontal)
                        result[i, j] = matrix[i, cols - 1 - j];
                    else
                        result[i, j] = matrix[rows - 1 - i, j];
                }
            }
            return result;
        }

        private static void Validate(T[,] matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            if (matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0)
                throw new ArgumentException("Матрица не должна быть пустой.");
        }

        public static void Print(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j],5}");
                }
                Console.WriteLine();
            }
        }
    }

    public static void Run()
    {
        int[,] matrix =
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        MatrixTransformer<int> transformer = new MatrixTransformer<int>();

        Console.WriteLine("Исходная матрица:");
        MatrixTransformer<int>.Print(matrix);

        Console.WriteLine("\nТранспонированная матрица:");
        MatrixTransformer<int>.Print(transformer.Transpose(matrix));

        Console.WriteLine("\nПоворот на 90 градусов:");
        MatrixTransformer<int>.Print(transformer.Rotate90(matrix));

        Console.WriteLine("\nОтражение по горизонтали:");
        MatrixTransformer<int>.Print(transformer.Flip(matrix, FlipDirection.Horizontal));

        Console.WriteLine("\nОтражение по вертикали:");
        MatrixTransformer<int>.Print(transformer.Flip(matrix, FlipDirection.Vertical));
    }
}
