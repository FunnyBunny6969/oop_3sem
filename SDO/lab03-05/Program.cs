using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 5: наследование и полиморфизм ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: иерархия сотрудников с премиями и стажем");
        Dop1.Run();

        Section("Дополнительное задание 2: геометрические фигуры с проверкой вложенности");
        Dop2.Run();

        Section("Дополнительное задание 3: транспортная иерархия с учетом времени суток");
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
    public class Employee
    {
        public string Name { get; }
        public decimal Salary { get; }
        public DateTime HireDate { get; }

        public Employee(string name, decimal salary, DateTime hireDate)
        {
            Name = name;
            Salary = salary;
            HireDate = hireDate;
        }

        public int GetExperience()
        {
            DateTime today = DateTime.Today;
            int years = today.Year - HireDate.Year;
            if (today.Month < HireDate.Month ||
                (today.Month == HireDate.Month && today.Day < HireDate.Day))
            {
                years--;
            }
            return Math.Max(0, years);
        }

        public virtual decimal CalculateBonus() => Salary * 0.01m * GetExperience();

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"  Сотрудник: {Name}, оклад {Salary:F2} руб., стаж {GetExperience()} лет, бонус {CalculateBonus():F2} руб.");
        }
    }

    public class Manager : Employee
    {
        public Manager(string name, decimal salary, DateTime hireDate)
            : base(name, salary, hireDate) { }

        public override decimal CalculateBonus() => Salary * 0.20m + base.CalculateBonus();

        public override void DisplayInfo()
        {
            Console.WriteLine($"  Менеджер: {Name}, оклад {Salary:F2} руб., стаж {GetExperience()} лет, бонус {CalculateBonus():F2} руб.");
        }
    }

    public class Developer : Employee
    {
        public int ProjectsCount { get; }

        public Developer(string name, decimal salary, DateTime hireDate, int projectsCount)
            : base(name, salary, hireDate)
        {
            ProjectsCount = projectsCount;
        }

        public override decimal CalculateBonus() =>
            Salary * 0.10m + 500m * ProjectsCount + base.CalculateBonus();

        public override void DisplayInfo()
        {
            Console.WriteLine($"  Разработчик: {Name}, оклад {Salary:F2} руб., стаж {GetExperience()} лет, проектов {ProjectsCount}, бонус {CalculateBonus():F2} руб.");
        }
    }

    public static void Run()
    {
        Employee[] employees =
        {
            new Manager("Анна Сидорова", 50000m, DateTime.Today.AddYears(-5)),
            new Developer("Петр Иванов", 40000m, DateTime.Today.AddYears(-3), 4),
            new Employee("Иван Петров", 30000m, DateTime.Today.AddYears(-1))
        };

        foreach (Employee employee in employees)
        {
            employee.DisplayInfo();
        }
    }
}
