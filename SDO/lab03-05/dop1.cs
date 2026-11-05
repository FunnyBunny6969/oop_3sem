using System;
using System.Linq;

public static class Dop1
{
    public class Employee
    {
        public string Name { get; }
        public decimal BaseSalary { get; }
        public DateTime HireDate { get; }

        public Employee(string name, decimal baseSalary, DateTime hireDate)
        {
            Name = name;
            BaseSalary = baseSalary;
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

        public virtual decimal CalculateSalary() => BaseSalary;

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"  {Name}: оклад {BaseSalary:F2}, стаж {GetExperience()} лет, итого {CalculateSalary():F2} руб.");
        }
    }

    public class Manager : Employee
    {
        public Manager(string name, decimal baseSalary, DateTime hireDate)
            : base(name, baseSalary, hireDate) { }

        public override decimal CalculateSalary() => BaseSalary * 1.20m;

        public override void DisplayInfo()
        {
            Console.WriteLine($"  Менеджер {Name}: оклад {BaseSalary:F2}, стаж {GetExperience()} лет, итого {CalculateSalary():F2} руб.");
        }
    }

    public class Developer : Employee
    {
        public Developer(string name, decimal baseSalary, DateTime hireDate)
            : base(name, baseSalary, hireDate) { }

        public override decimal CalculateSalary() => BaseSalary * 1.10m + 500m * GetExperience();

        public override void DisplayInfo()
        {
            Console.WriteLine($"  Разработчик {Name}: оклад {BaseSalary:F2}, стаж {GetExperience()} лет, итого {CalculateSalary():F2} руб.");
        }
    }

    public class SalesPerson : Employee
    {
        public decimal SalesVolume { get; }

        public SalesPerson(string name, decimal baseSalary, DateTime hireDate, decimal salesVolume)
            : base(name, baseSalary, hireDate)
        {
            SalesVolume = salesVolume;
        }

        public override decimal CalculateSalary() => BaseSalary + SalesVolume * 0.05m;

        public override void DisplayInfo()
        {
            Console.WriteLine($"  Продавец {Name}: оклад {BaseSalary:F2}, продажи {SalesVolume:F2}, стаж {GetExperience()} лет, итого {CalculateSalary():F2} руб.");
        }
    }

    public static decimal GetTotalSalary(Employee[] employees) => employees.Sum(e => e.CalculateSalary());

    public static Employee FindHighestPaid(Employee[] employees) =>
        employees.OrderByDescending(e => e.CalculateSalary()).First();

    public static void Run()
    {
        Employee[] employees =
        {
            new Manager("Анна Сидорова", 50000m, DateTime.Today.AddYears(-6)),
            new Developer("Петр Иванов", 40000m, DateTime.Today.AddYears(-3)),
            new SalesPerson("Ольга Кузнецова", 35000m, DateTime.Today.AddYears(-2), 500000m),
            new Employee("Иван Петров", 30000m, DateTime.Today.AddYears(-1))
        };

        foreach (Employee employee in employees) employee.DisplayInfo();

        Employee highest = FindHighestPaid(employees);
        Console.WriteLine($"\nОбщий фонд зарплат: {GetTotalSalary(employees):F2} руб.");
        Console.WriteLine($"Максимальная зарплата: {highest.Name} — {highest.CalculateSalary():F2} руб.");
    }
}
