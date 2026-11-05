using System;
using System.Collections.Generic;

public static class Dop1
{
    public class Calculator
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;

        public double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Деление на ноль невозможно.");
            return a / b;
        }
    }

    private static double Evaluate(Calculator calculator, string expression)
    {
        string[] parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3) throw new FormatException("Ожидается формат: число операция число.");

        double a = double.Parse(parts[0]);
        double b = double.Parse(parts[2]);

        return parts[1] switch
        {
            "+" => calculator.Add(a, b),
            "-" => calculator.Subtract(a, b),
            "*" => calculator.Multiply(a, b),
            "/" => calculator.Divide(a, b),
            _ => throw new InvalidOperationException($"Неизвестная операция: {parts[1]}")
        };
    }

    public static void Run()
    {
        Calculator calculator = new Calculator();
        List<string> expressions = new List<string>
        {
            "10 + 5",
            "20 / 4",
            "7 / 0",
            "abc + 3",
            "8 ^ 2",
            "2 * 3"
        };

        foreach (string expression in expressions)
        {
            try
            {
                double result = Evaluate(calculator, expression);
                Console.WriteLine($"{expression} = {result:F2}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"{expression}: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{expression}: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"{expression}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("  Операция завершена, калькулятор продолжает работу.");
            }
        }
    }
}
