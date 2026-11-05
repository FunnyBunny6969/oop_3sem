using System;
using System.Collections.Generic;
using System.Globalization;

public static class Dop1
{
    public class LambdaCalculator
    {
        private readonly Dictionary<char, Func<double, double, double>> _operations =
            new Dictionary<char, Func<double, double, double>>();

        private readonly Dictionary<string, Func<double, double>> _functions =
            new Dictionary<string, Func<double, double>>(StringComparer.OrdinalIgnoreCase);

        public LambdaCalculator()
        {
            RegisterOperation('+', (a, b) => a + b);
            RegisterOperation('-', (a, b) => a - b);
            RegisterOperation('*', (a, b) => a * b);
            RegisterOperation('/', (a, b) =>
            {
                if (b == 0) throw new DivideByZeroException("Деление на ноль.");
                return a / b;
            });

            _functions["sin"] = x => Math.Sin(x * Math.PI / 180);
            _functions["cos"] = x => Math.Cos(x * Math.PI / 180);
            _functions["sqrt"] = x =>
            {
                if (x < 0) throw new ArgumentException("Корень из отрицательного числа.");
                return Math.Sqrt(x);
            };
        }

        public void RegisterOperation(char symbol, Func<double, double, double> operation)
        {
            _operations[symbol] = operation;
        }

        public double Calculate(string expression)
        {
            string[] parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 && parts[1].Length == 1)
            {
                char symbol = parts[1][0];
                if (!_operations.TryGetValue(symbol, out Func<double, double, double>? operation))
                    throw new InvalidOperationException($"Операция '{symbol}' не зарегистрирована.");

                double left = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double right = double.Parse(parts[2], CultureInfo.InvariantCulture);
                return operation(left, right);
            }

            if (parts.Length == 2)
            {
                if (!_functions.TryGetValue(parts[0], out Func<double, double>? function))
                    throw new InvalidOperationException($"Функция '{parts[0]}' не найдена.");

                double value = double.Parse(parts[1], CultureInfo.InvariantCulture);
                return function(value);
            }

            throw new FormatException("Неверный формат выражения.");
        }
    }

    public static void Run()
    {
        LambdaCalculator calculator = new LambdaCalculator();
        string[] expressions =
        {
            "5 + 3", "10 * 4", "20 / 0", "sin 30", "sqrt 16", "sqrt -4", "unknown 5"
        };

        foreach (string expression in expressions)
        {
            try
            {
                Console.WriteLine($"{expression} = {calculator.Calculate(expression):F4}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{expression}: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
