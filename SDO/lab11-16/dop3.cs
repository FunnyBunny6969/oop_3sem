using System;
using System.Linq;
using System.Reflection;

public static class Dop3
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SinceAttribute : Attribute
    {
        public int Version { get; }
        public SinceAttribute(int version) => Version = version;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DeprecatedSinceAttribute : Attribute
    {
        public int Version { get; }
        public string Alternative { get; }

        public DeprecatedSinceAttribute(int version, string alternative)
        {
            Version = version;
            Alternative = alternative;
        }
    }

    public class Calculator
    {
        [Since(1)]
        public int Add(int a, int b) => a + b;

        [Since(2)]
        public int Multiply(int a, int b) => a * b;

        [Since(1)]
        [DeprecatedSince(3, "Multiply")]
        public int MultiplyOld(int a, int b) => a * b;

        [Since(4)]
        public int Divide(int a, int b) => b == 0 ? throw new DivideByZeroException() : a / b;
    }

    public class VersionAnalyzer<T>
    {
        private readonly int _currentVersion;

        public VersionAnalyzer(int currentVersion) => _currentVersion = currentVersion;

        public void Analyze()
        {
            foreach (MethodInfo method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                SinceAttribute? since = method.GetCustomAttribute<SinceAttribute>();
                if (since == null) continue;

                DeprecatedSinceAttribute? deprecated = method.GetCustomAttribute<DeprecatedSinceAttribute>();

                if (since.Version > _currentVersion)
                {
                    Console.WriteLine($"  {method.Name}: появится в версии {since.Version} (недоступен)");
                }
                else if (deprecated != null)
                {
                    Console.WriteLine($"  {method.Name}: устарел с версии {deprecated.Version}, используйте {deprecated.Alternative}");
                }
                else
                {
                    Console.WriteLine($"  {method.Name}: доступен с версии {since.Version}");
                }
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("Анализ для текущей версии 3:");
        new VersionAnalyzer<Calculator>(3).Analyze();

        Console.WriteLine("\nАнализ для текущей версии 4:");
        new VersionAnalyzer<Calculator>(4).Analyze();
    }
}
